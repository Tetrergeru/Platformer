using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace Platformer.Physics
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public static class DirectionAttributes
    {
        public static Direction Reverse(Direction direction)
        {
            switch (direction)
            {
                case Direction.Down:
                    return Direction.Up;
                case Direction.Up:
                    return Direction.Down;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                default:
                    throw new InvalidProgramException("Поворот отсутствует");
            }
        }

        public static Direction FromVector(Vector vector)
        {
            if (Abs(vector.x) > Abs(vector.y))
            {
                if (vector.x < 0)
                    return Direction.Left;
                return Direction.Right;
            }
            if (vector.y < 0)
                return Direction.Up;
            return Direction.Down;
        }

        public static Vector ToVector(Direction direction)
        {
            switch (direction)
            {
                case Direction.Down:
                    return new Vector { x = 0, y = 1 };
                case Direction.Up:
                    return new Vector { x = 0, y = -1 };
                case Direction.Left:
                    return new Vector { x = -1, y = 0 };
                case Direction.Right:
                    return new Vector { x = 1, y = 0 };
                default:
                    throw new InvalidProgramException("Поворот отсутствует");
            }
        }
    }
}
