using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolver
{
    public enum PaneState : byte
    {
        Blank = 0,
        Occupied = 1,
        Moving = 3,

        UpOrDownward = 4,
        DownOrRightward = 8,

        MovingLeftward = Moving,
        MovingRightward = Moving | DownOrRightward,
        MovingUpward = Moving | UpOrDownward,
        MovingDownward = Moving | UpOrDownward | DownOrRightward,

        Invalid = 0xFF,
    }
}
