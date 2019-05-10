using System;
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
        public static Direction Reverse(this Direction direction)
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
                    throw new ArgumentException("Поворот отсутствует");
            }
        }

        public static Direction FromVector(Vector vector)
        {
            if (Abs(vector.x) > Abs(vector.y))
                return vector.x < 0 ? Direction.Left : Direction.Right;
            return vector.y < 0 ? Direction.Up : Direction.Down;
        }

        public static Vector ToVector(this Direction direction)
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
                    throw new ArgumentException("Поворот отсутствует");
            }
        }
    }
}
