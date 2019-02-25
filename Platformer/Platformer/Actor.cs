using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class Actor : Entity
    {
        public enum Direction { Right, Left };

        private const double MaxHorizontalVelocity = 20;

        private const double MaxVerticalVelocity = 100;

        protected Vector acceleration = new Vector { x = 0, y = 98 };

        public Vector velocity;

        public Actor(World context, HitBox hitbox) : base(context, hitbox)
        {

        }

        public void Pull(Vector force)
        {
            velocity += force;
        }

        const double RunningSpeed = 5;

        private const double JumpHeight = 17;

        public void Run(Direction direction)
        {
            if (direction == Direction.Right)
                Pull(new Vector { x = RunningSpeed, y = 0, });
            else
                Pull(new Vector { x = -RunningSpeed, y = 0, });
        }

        private void CutVelocity()
        {
            if (velocity.x > MaxHorizontalVelocity)
                velocity.x = MaxHorizontalVelocity;
            if (velocity.x < -MaxHorizontalVelocity)
                velocity.x = -MaxHorizontalVelocity;

            if (velocity.y > MaxVerticalVelocity)
                velocity.y = MaxVerticalVelocity;
            if (velocity.y < -MaxVerticalVelocity)
                velocity.y = -MaxVerticalVelocity;
        }

        private bool MovementIsPossble(Vector velocity)
        {
            var tempHitbox = new HitBox(hitbox.X + velocity.x, hitbox.Y + velocity.y, hitbox.Width, hitbox.Height);

            foreach (var e in context.Entities)
                if (e != this && e.Intersects(tempHitbox))
                {
                    return false;
                }
            return true;
        }

        private bool FreeFromDown()
            => MovementIsPossble(new Vector { x = 0, y = 3});

        public void Jump()
        {
            if (!FreeFromDown())
                velocity += new Vector { x = 0, y = -JumpHeight };
        }

        public void TryToStop()
        {
            if (!FreeFromDown())
                velocity.x = 0;
        }

        public void Move(double deltaTime)
        {
            CutVelocity();

            if (MovementIsPossble(velocity.ZeroY()))
                Move(velocity.ZeroY());
            else
                velocity = velocity.ZeroX();

            if (MovementIsPossble(velocity.ZeroX()))
                Move(velocity.ZeroX());
            else
                velocity = velocity.ZeroY();

            velocity += acceleration * deltaTime;
            velocity.x *= Math.Pow(0.01, deltaTime);
        }
    }
}
