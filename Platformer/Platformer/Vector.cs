using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    struct Vector
    {
        public double x;

        public double y;

        public static Vector operator +(Vector first, Vector second)
            => new Vector
            {
                x = first.x + second.x,
                y = first.y + second.y,
            };

        public static Vector operator *(Vector vector, double mutiplier)
            => new Vector
            {
                x = vector.x * mutiplier,
                y = vector.y * mutiplier,
            };

        public static Vector Zero()
            => new Vector { x = 0, y = 0 };

        public Vector ZeroX()
            => new Vector { x = 0, y = y };

        public Vector ZeroY()
            => new Vector { x = x, y = 0 };

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }
}
