using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Evolver
{
    public class MutantManager
    {
        public delegate void ProgressCallback(int progress);

        private WorldDbContext dbContext;
        private MutantFactory mutantFactory;
        private string mutantsPath;
        private string mutantsBinPath;
        private Mutant[] mutants;

        public MutantManager(WorldDbContext dbContext, string worldPath)
        {
            this.dbContext = dbContext;
            mutantFactory = new MutantFactory(dbContext.MutantSize);
            mutantsPath = Path.Combine(worldPath, "mutants");
            mutantsBinPath = Path.Combine(mutantsPath, "all.bin");
        }

        public void CreateMutants(ProgressCallback progressCallback)
        {
            progressCallback(1);
            Directory.CreateDirectory(mutantsPath);
            dbContext.Configuration.AutoDetectChangesEnabled = false;

            using (var file = new FileStream(mutantsBinPath, FileMode.Create))
            {
                using (var writer = new BinaryWriter(file))
                {
                    mutants = new Mutant[dbContext.NumMutants];
                    for (var i = 0; i < dbContext.NumMutants; ++i)
                    {
                        mutants[i] = mutantFactory.CreateMutant(i);
                        mutants[i].WriteBytes(writer);
                        dbContext.Mutants.Add(new MutantStat { Id = i });
                        progressCallback(1 + (i + 1) * 97 / dbContext.NumMutants);
                    }
                }
            }

            progressCallback(99);
            dbContext.Configuration.AutoDetectChangesEnabled = true;
            dbContext.SaveChanges();
            progressCallback(100);
        }

        public void Recreate(int mutantId)
        {
            mutants[mutantId] = mutantFactory.CreateMutant(mutantId);
        }

        public int Replicate(int sourceId, int destinationId, int maxMutatedBytes)
        {
            Random random = new Random();
            var sourceMutant = mutants[sourceId];
            var newBytes = sourceMutant.BytesList();
            int mutatedBytes = random.Next(maxMutatedBytes + 1);
            for (int i = 0; i < mutatedBytes; ++i)
            {
                var mutateMethod = random.Next(6);
                var position = random.Next(dbContext.MutantSize);
                switch (mutateMethod)
                {
                    case 0:
                    case 1:
                        {
                            var value = (byte)random.Next(0, 256);
                            newBytes[position] = value;
                        }
                        break;
                    case 2:
                        {
                            var value = (byte)random.Next(0, 256);
                            newBytes.RemoveAt(position);
                            newBytes.Add(value);
                        }
                        break;
                    case 3:
                        {
                            var value = (byte)random.Next(0, 256);
                            newBytes.Insert(position, value);
                            newBytes.RemoveAt(mutatedBytes);
                        }
                        break;
                    case 4: // Copy
                        {
                            var source = random.Next(0, dbContext.MutantSize);
                            if (source != position)
                                newBytes[position] = newBytes[source];
                        }
                        break;
                    case 5: // Swap
                        {
                            var other = random.Next(0, dbContext.MutantSize);
                            if (other != position)
                            {
                                var temp = newBytes[position];
                                newBytes[position] = newBytes[other];
                                newBytes[other] = temp;
                            }
                        }
                        break;
                }
            }
            mutants[destinationId] = new Mutant(destinationId, newBytes.ToArray());
            return mutatedBytes;
        }

        public void Flush()
        {
            using (var file = new FileStream(mutantsBinPath, FileMode.Open, FileAccess.Write))
            {
                using (var writer = new BinaryWriter(file))
                {
                    writer.Seek(0, SeekOrigin.Begin);
                    foreach (var mutant in mutants)
                    {
                        mutant.WriteBytes(writer);
                    }
                }
            }
        }

        public void LoadMutants()
        {
            using (var stream = new FileStream(mutantsBinPath, FileMode.Open, FileAccess.Read))
            {
                if (stream.Length != dbContext.NumMutants * dbContext.MutantSize)
                    throw new Exception("Incomplete mutants binary");

                mutants = new Mutant[dbContext.NumMutants];
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    for (var i = 0; i < dbContext.NumMutants; ++i)
                    {
                        mutants[i] = new Mutant(i, reader.ReadBytes(dbContext.MutantSize));
                    }
                }
            }
        }

        public Mutant LoadMutant(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(stream))
                {
                    return Mutant.CreateMutantWithText(reader, dbContext.MutantSize);
                }
            }
        }

        public Mutant GetMutant(int id)
        {
            return mutants[id];
        }

        internal void ResizeMutants(int newSize, ProgressCallback progressCallback)
        {
            progressCallback(1);
            using (var file = new FileStream(mutantsBinPath, FileMode.Open, FileAccess.Write))
            {
                using (var writer = new BinaryWriter(file))
                {
                    writer.Seek(0, SeekOrigin.Begin);
                    for (var i = 0; i < dbContext.NumMutants; ++i)
                    {
                        mutants[i].Resize(newSize);
                        mutants[i].WriteBytes(writer);
                        progressCallback(1 + (i + 1) * 98 / dbContext.NumMutants);
                    }
                }
            }
            dbContext.ChangeMutantSize(newSize);
            progressCallback(100);
        }
    }
}
