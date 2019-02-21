using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    struct Vector
    {
        double x;

        double y;

        public static Vector operator +(Vector first, Vector second)
            => new Vector
            {
                x = first.x + second.x,
                y = first.y + second.y
            };
    }
}
