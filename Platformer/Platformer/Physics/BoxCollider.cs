using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace Platformer.Physics
{
    class BoxCollider : ICollider
    {
        public double x = 0;
        public double y = 0;
        public double width = 0;
        public double height = 0;

        private double IntersectLines(double begin1, double end1, double begin2, double end2)
        {
            double res = Abs(begin1 - end2) + Abs(begin2 - end1) - Abs(begin2 - begin1) - Abs(end2 - end1);
            return res / 2;
        }

        ICollider ICollider.CollisionWith(ICollider collider)
        {
            if (collider is BoxCollider box)
                return new BoxCollider
                {
                    x = Max(Min(x, x + width), Min(box.x, box.x + box.width)),
                    y = Max(Min(y, y + height), Min(box.y, box.y + box.height)),
                    width = IntersectLines(x, x + width, box.x, box.x + box.width),
                    height = IntersectLines(y, y + height, box.y, box.y + box.height)
                };
            else
                throw new NotImplementedException("Неизвестный коллайдер " + collider.GetType().ToString());
        }

        BoxCollider ICollider.AxisAlignedBoundingBox()
        {
            return this;
        }

        public double Volume()
        {
            return width * height;
        }

        Vector ICollider.Center()
        {
            return new Vector { x = x + width / 2, y = y + height / 2 };
        }

        public void Move(Vector vector)
        {
            x += vector.x;
            y += vector.y;
        }
    }
}
