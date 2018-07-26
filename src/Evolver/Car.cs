using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolver
{
    public class Car
    {
        public Position Position { get; set; }
        public Position Goal { get; set; }
        public PaneState Direction { get; set; }

        public Car()
        {
            Position = new Position();
            Goal = new Position();
        }

        public bool IsDirectionRight()
        {
            switch (Direction)
            {
                case PaneState.MovingUpward:
                    return Goal.Y <= Position.Y;
                case PaneState.MovingDownward:
                    return Goal.Y >= Position.Y;
                case PaneState.MovingLeftward:
                    return Goal.X <= Position.X;
                case PaneState.MovingRightward:
                    return Goal.X >= Position.X;
            }
            throw new Exception("Unexpected direction");
        }

        public Position NextPosition()
        {
            Position newPos = Position;
            switch (Direction)
            {
                case PaneState.MovingUpward:
                    newPos.Y -= 1;
                    break;
                case PaneState.MovingDownward:
                    newPos.Y += 1;
                    break;
                case PaneState.MovingLeftward:
                    newPos.X -= 1;
                    break;
                case PaneState.MovingRightward:
                    newPos.X += 1;
                    break;
            }

            return newPos;
        }

        public void TurnBack()
        {
            switch (Direction)
            {
                case PaneState.MovingUpward:
                    Direction = PaneState.MovingDownward;
                    break;
                case PaneState.MovingDownward:
                    Direction = PaneState.MovingUpward;
                    break;
                case PaneState.MovingLeftward:
                    Direction = PaneState.MovingRightward;
                    break;
                case PaneState.MovingRightward:
                    Direction = PaneState.MovingLeftward;
                    break;
            }
        }

        public void TurnLeft()
        {
            switch (Direction)
            {
                case PaneState.MovingUpward:
                    Direction = PaneState.MovingLeftward;
                    break;
                case PaneState.MovingDownward:
                    Direction = PaneState.MovingRightward;
                    break;
                case PaneState.MovingLeftward:
                    Direction = PaneState.MovingDownward;
                    break;
                case PaneState.MovingRightward:
                    Direction = PaneState.MovingUpward;
                    break;
            }
        }

        public void TurnRight()
        {
            switch (Direction)
            {
                case PaneState.MovingUpward:
                    Direction = PaneState.MovingRightward;
                    break;
                case PaneState.MovingDownward:
                    Direction = PaneState.MovingLeftward;
                    break;
                case PaneState.MovingLeftward:
                    Direction = PaneState.MovingUpward;
                    break;
                case PaneState.MovingRightward:
                    Direction = PaneState.MovingDownward;
                    break;
            }
        }

        public Position RelativeGoal()
        {
            return RelativePosition(Goal);
        }

        public Position RelativeUp()
        {
            return RelativePosition(Position.Up());
        }

        public Position RelativeDown()
        {
            return RelativePosition(Position.Down());
        }

        public Position RelativeLeft()
        {
            return RelativePosition(Position.Left());
        }

        public Position RelativeRight()
        {
            return RelativePosition(Position.Right());
        }

        public Position RelativePosition(Position pos)
        {
            /* Y
             * ^
             * |
             * |
             * |
             * -------> X
             */
            Position relative = pos - Position;
            switch (Direction)
            {
                case PaneState.MovingUpward:
                    relative.Y = (sbyte)-relative.Y;
                    break;
                case PaneState.MovingDownward:
                    relative.X = (sbyte)-relative.X;
                    break;
                case PaneState.MovingLeftward:
                    relative.X = (sbyte)-relative.Y;
                    relative.Y = (sbyte)-relative.X;
                    break;
                case PaneState.MovingRightward:
                    relative.X = (sbyte)relative.Y;
                    relative.Y = (sbyte)relative.X;
                    break;
            }
            return relative;
        }

        public Position AbsolutePosition(Position relative)
        {
            /* Y
             * ^
             * |
             * |
             * |
             * -------> X
             */
            Position absolute = Position;
            switch (Direction)
            {
                case PaneState.MovingUpward:
                    absolute.Y -= relative.Y;
                    absolute.X += relative.X;
                    break;
                case PaneState.MovingDownward:
                    absolute.Y += relative.Y;
                    absolute.X -= relative.X;
                    break;
                case PaneState.MovingLeftward:
                    absolute.Y -= relative.X;
                    absolute.X -= relative.Y;
                    break;
                case PaneState.MovingRightward:
                    absolute.Y += relative.X;
                    absolute.X += relative.Y;
                    break;
            }
            return absolute;
        }

    }
}
