using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolver
{
    public struct EvolverSettings
    {
        public int mapSize;
        public int numBlocksPerRow;
        public int blockSize;
        public int maxInstructionsToRun;
        public int eliminateThreshold;
        public int numParallelThreads;
        public int mostBytesToMutate;
        public bool driveThru;
    }
}
