using System;

namespace Platformer
{
    class Entity
    {
        protected HitBox hitbox;

        protected World context;

        public Entity(World context, HitBox hitbox)
        {
            this.context = context;
            this.hitbox = hitbox;
        }

        public bool Intersects(Entity other)
            => hitbox.Intersects(other.hitbox);
    }
}
