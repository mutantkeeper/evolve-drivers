using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolver
{
    public struct Position: IEquatable<Position>
    {
        public sbyte X { get; set; }
        public sbyte Y { get; set; }
        public Position(sbyte x, sbyte y)
        {
            X = x;
            Y = y;
        }

        public Position Left()
        {
            return new Position((sbyte)(X - 1), Y);
        }

        public Position Right()
        {
            return new Position((sbyte)(X + 1), Y);
        }

        public Position Up()
        {
            return new Position(X, (sbyte)(Y - 1));
        }

        public Position Down()
        {
            return new Position(X, (sbyte)(Y + 1));
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is Position))
                return false;

            return Equals((Position)obj);
        }

        public override int GetHashCode()
        {
            return ((int)X << 8) + Y;
        }

        public bool Equals(Position other)
        {
            return X == other.X & Y == other.Y;
        }

        public static Position operator-(Position a, Position b)
        {
            return new Position((sbyte)(a.X - b.X), (sbyte)(a.Y - b.Y));
        }
    }
}
