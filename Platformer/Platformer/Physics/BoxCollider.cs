using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Physics
{
    class BoxCollider : ICollider
    {
        public double x;
        public double y;
        public double width;
        public double height;

        ICollider ICollider.CollisionWith(ICollider collider)
        {
            throw null;
        }

        BoxCollider ICollider.AxisAlignedBoundingBox()
        {
            throw null;
        }

        double ICollider.Volume()
        {
            throw null;
        }
    }
}
