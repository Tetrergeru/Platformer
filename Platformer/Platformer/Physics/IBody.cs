using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Physics
{
    interface IBody
    {
        void Pull(Vector vector);
        void Accelerate(Vector vector);
        void Move(Vector vector);
    }
}
