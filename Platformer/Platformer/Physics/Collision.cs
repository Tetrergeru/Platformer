using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
using static Platformer.Physics.DirectionAttributes;

namespace Platformer.Physics
{
    static class Collision
    {
        public static void Interaction(Body body1, Body body2, double deltaTime)
        {
            if (!body1.MovementRecipient && !body2.MovementRecipient)
                return;
            if (!body1.MovementEmitter && !body2.MovementEmitter)
                return;

            ICollider collision = body1.collider.CollisionWith(body2.collider);
            if (collision is BoxCollider box)
            {
                if (box.Volume() < 1e-10)
                    return;
                Vector dist = body1.Center() - body2.Center();
                Vector deltaVelocity = body1.Velocity - body2.Velocity;
                dist.x = Sign(dist.x);
                dist.y = Sign(dist.y);

                Direction direction;

                if (box.Width > box.Height)
                    if (dist.y > 0)
                        direction = Direction.Up;
                    else
                        direction = Direction.Down;
                else
                    if (dist.x > 0)
                        direction = Direction.Left;
                    else
                        direction = Direction.Right;

                if (body1.MovementRecipient && body2.MovementEmitter)
                    InteractionWith(body1, body2, box, direction, deltaVelocity, deltaTime);
                
                if (body2.MovementRecipient && body1.MovementEmitter)
                    InteractionWith(body2, body1, box, Reverse(direction), deltaVelocity * -1, deltaTime);
            }
            else
                throw new NotImplementedException("Неизвестный коллайдер " + collision.GetType().ToString());
        }

        public static void InteractionWith(Body body, Body target, ICollider collision, Direction direction, Vector deltaVelocity, double deltaTime)
        {
            Vector directionVector = ToVector(Reverse(direction));
            Vector force = directionVector * collision.Volume() * 2000000;

            Vector absDirection = new Vector { x = Abs(directionVector.x), y = Abs(directionVector.y) };
            Vector absDirectionRotate = new Vector { x = absDirection.y, y = absDirection.x};
            body.Pull(force);
            body.Pull(absDirection * deltaVelocity * -10000);
            body.Pull(absDirectionRotate * deltaVelocity * -200);

            body.CollisionWith(target, direction);
        }
    }
}
