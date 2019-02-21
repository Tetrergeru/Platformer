using System;
using System.Drawing;

namespace Platformer
{
    class HitBox
    {
        protected Rectangle rectangle;

        public bool Intersects(HitBox other)
            => rectangle.IntersectsWith(other.rectangle);
    }
}
