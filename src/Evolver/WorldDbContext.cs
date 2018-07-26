using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolver
{
    [Table("Parameters")]
    public class Parameter
    {
        [Key]
        public string Key { get; set; }
        public Int64 Value { get; set; }
    }

    [Table("Statistics")]
    public class Statistic
    {
        [Key]
        public string Key { get; set; }
        public Int64 Value { get; set; }
    }

    public class WorldDbContext : DbContext
    {
        public DbSet<MutantStat> Mutants { get; set; }
        public int MutantSize { get; private set; }
        public int NumMutants { get; private set; }
        public Int64 TotalReplicates
        {
            get { return Statistics.Find("TotalReplicates").Value; }
            set { Statistics.Find("TotalReplicates").Value = value; }
        }
        public Int64 TotalTurns
        {
            get { return Statistics.Find("TotalTurns").Value; }
            set { Statistics.Find("TotalTurns").Value = value; }
        }
        public Int64 TotalMutatedBytes
        {
            get { return Statistics.Find("TotalMutatedBytes").Value; }
            set { Statistics.Find("TotalMutatedBytes").Value = value; }
        }

        public Int64 MapResets { get; set; }

        public DbSet<Parameter> Parameters { get; set; }
        public DbSet<Statistic> Statistics { get; set; }

        public WorldDbContext(System.Data.Common.DbConnection connection, bool loadData)
            : base(connection, true)
        {
            if (loadData)
            {
                MutantSize = (int)Parameters.Find("mutantSize").Value;
                NumMutants = (int)Parameters.Find("numMutants").Value;
                if (Mutants.Count() != NumMutants)
                {
                    throw new Exception("Incomplete Database");
                }
            }
        }

        public void Create(int numMutants, int mutantSize)
        {
            MutantSize = mutantSize;
            NumMutants = numMutants;
            
            // Entity Frame 6 doesn't create tables when used with SQLite3, so we have to do it ourselves.
            using (var transaction = this.Database.BeginTransaction())
            {
                this.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                    "CREATE TABLE IF NOT EXISTS Parameters (Key TEXT PRIMARY KEY NOT NULL, Value INTEGER NOT NULL)");
                this.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                    "CREATE TABLE IF NOT EXISTS Statistics (Key TEXT PRIMARY KEY NOT NULL, Value INTEGER NOT NULL)");
                this.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                    "CREATE TABLE IF NOT EXISTS Mutants " +
                    "(Id INTEGER PRIMARY KEY NOT NULL, " +
                    "Score INTEGER  NOT NULL DEFAULT 0," +
                    "Generation INTEGER NOT NULL DEFAULT 0," +
                    "Turns INTEGER NOT NULL DEFAULT 0," +
                    "ScorePer100Turns REAL NOT NULL DEFAULT 0.0)");
                this.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                    "CREATE INDEX IF NOT EXISTS IndexScore ON Mutants (ScorePer100Turns)");
                transaction.Commit();
            }
            Parameters.Add(new Parameter { Key = "numMutants", Value = numMutants });
            Parameters.Add(new Parameter { Key = "mutantSize", Value = mutantSize });
            Statistics.Add(new Statistic { Key = "TotalReplicates", Value = 0 });
            Statistics.Add(new Statistic { Key = "TotalTurns", Value = 0 });
            Statistics.Add(new Statistic { Key = "TotalMutatedBytes", Value = 0 });
            SaveChanges();
        }

        public void ChangeMutantSize(int newSize)
        {
            Parameters.Find("mutantSize").Value = newSize;
            SaveChanges();
        }
    }
}
