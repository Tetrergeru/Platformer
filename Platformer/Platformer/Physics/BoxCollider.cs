using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace Platformer.Physics
{
    class BoxCollider : ICollider, IRectangle
    {
        public double X { get; set;} 

        public double Y { get; set; }

        public double X2 => X + Width;

        public double Y2 => Y + Height;

        public double Width { get; set; }

        public double Height { get; set; }

        public Vector Coordinates => new Vector { x = X, y = Y };

        public BoxCollider() { }

        public BoxCollider(IRectangle rectangle)
        {
            X = rectangle.X;
            Y = rectangle.Y;
            Width = rectangle.Width;
            Height = rectangle.Height;
        }

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
                    X = Max(Min(X, X + Width), Min(box.X, box.X + box.Width)),
                    Y = Max(Min(Y, Y + Height), Min(box.Y, box.Y + box.Height)),
                    Width = IntersectLines(X, X + Width, box.X, box.X + box.Width),
                    Height = IntersectLines(Y, Y + Height, box.Y, box.Y + box.Height)
                };
            else
                throw new NotImplementedException("Неизвестный коллайдер " + collider.GetType().ToString());
        }

        IRectangle ICollider.AxisAlignedBoundingBox()
        {
            return this;
        }

        public double Volume()
        {
            return Width * Height;
        }

        Vector ICollider.Center()
        {
            return new Vector { x = X + Width / 2, y = Y + Height / 2 };
        }

        public void Move(Vector vector)
        {
            X += vector.x;
            Y += vector.y;
        }

        public void Resize(Vector ratio)
        {
            double newWidth = Width * ratio.x;
            double newHeight = Height * ratio.y;
            X -= (newWidth - Width) / 2;
            Y -= (newHeight- Height) / 2;
            Height = newHeight;
            Width = newWidth;
        }
    }
}
