using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolver
{
    public class Map
    {
        public sbyte Width { get; private set; }
        public sbyte Height { get; private set; }
        public PaneState this[Position pos]
        {
            get
            {
                return this[pos.X, pos.Y];
            }
            set
            {
                this[pos.X, pos.Y] = value;
            }
        }

        public PaneState this[sbyte x, sbyte y]
        {
            get
            {
                if (x >= 0 && x < Width && y >= 0 && y < Height)
                {
                    return panes[x, y];
                }
                return PaneState.Invalid;
            }
            set
            {
                panes[x, y] = value;
            }
        }

        private PaneState[,] panes;

        public Map(sbyte width, sbyte height)
        {
            Width = width;
            Height = height;
            panes = new PaneState[width, height];
        }
    }
}
