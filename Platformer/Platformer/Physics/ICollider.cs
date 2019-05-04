using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Physics
{
    interface ICollider
    {
        ICollider CollisionWith(ICollider collider);
        BoxCollider AxisAlignedBoundingBox();
        void Move(Vector vector);
        double Volume();
        Vector Center();
    }
}
