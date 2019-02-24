using System;
using System.Windows;
using System.Drawing;

namespace Platformer
{
    class HitBox
    {
        protected Rect rectangle;

        public double X { get { return rectangle.X; } }

        public double Y { get { return rectangle.Y; } }

        public double Width { get { return rectangle.Width; } }

        public double Height { get { return rectangle.Height; } }

        public HitBox(double x, double y, double w, double h)
        {
            rectangle = new Rect(x, y, w, h);
        }

        public bool Intersects(HitBox other)
            => rectangle.IntersectsWith(other.rectangle);

        public void Move(Vector velocity)
        {
            rectangle.X += velocity.x;
            rectangle.Y += velocity.y;
        }

        public Rectangle ToDrawing()
            => new Rectangle((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height);

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
