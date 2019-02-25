using System;

namespace Platformer
{
    class Entity
    {
        protected HitBox hitbox;

        public HitBox Hitbox
        {
            get { return hitbox; }
        }

        protected World context;

        public Entity(World context, HitBox hitbox)
        {
            this.context = context;
            this.hitbox = hitbox;
        }

        public bool Intersects(Entity other)
            => hitbox.Intersects(other.hitbox);

        public bool Intersects(HitBox other)
            => hitbox.Intersects(other);

        public void Move(Vector velocity)
            => hitbox.Move(velocity);
    }
}
