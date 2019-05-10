using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Physics
{
    public interface ICollider
    {
        ICollider CollisionWith(ICollider collider);

        IRectangle AxisAlignedBoundingBox();

        void Move(Vector vector);

        double Area { get; }

        Vector Center { get; }

        void Resize(Vector ratio);
    }
}
