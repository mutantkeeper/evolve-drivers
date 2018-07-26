using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolver
{
    class MutantFactory
    {
        private int mutantSize;

        public MutantFactory(int mutantSize)
        {
            Debug.Assert(mutantSize > 0);
            this.mutantSize = mutantSize;
        }

        public Mutant CreateMutant(int id)
        {
            Random random = new Random();
            byte[] bytes = new byte[mutantSize];
            random.NextBytes(bytes);
            return new Mutant(id, bytes);
        }
    }
}
