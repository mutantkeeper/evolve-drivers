using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolver
{
    public abstract class Player
    {
        public abstract void TakeTurn();
        public abstract void HandleClick(int space);
    }
}
