using System;
using static System.Math;

namespace Platformer.Physics
{
    internal static class Collision
    {
        public static void Interaction(Body body1, Body body2, double deltaTime)
        {
            if (!body1.MovementRecipient && !body2.MovementRecipient)
                return;
            if (!body1.MovementEmitter && !body2.MovementEmitter)
                return;

            var collision = body1.collider.CollisionWith(body2.collider);
            if (collision is BoxCollider box)
            {
                if (box.Area < 1e-10)
                    return;
                var dist = body1.Center - body2.Center;
                var deltaVelocity = body1.Velocity - body2.Velocity;
                dist.x = Sign(dist.x);
                dist.y = Sign(dist.y);

                Direction direction;

                if (box.Width > box.Height)
                    direction = dist.y > 0 ? Direction.Up : Direction.Down;
                else
                    direction = dist.x > 0 ? Direction.Left : Direction.Right;

                if (body1.MovementRecipient && body2.MovementEmitter)
                    InteractionWith(body1, body2, box, direction, deltaVelocity, deltaTime);

                if (body2.MovementRecipient && body1.MovementEmitter)
                    InteractionWith(body2, body1, box, direction.Reverse(), deltaVelocity * -1, deltaTime);
            }
            else
            {
                throw new NotImplementedException("Неизвестный коллайдер " + collision.GetType());
            }
        }

        public static void InteractionWith(Body body, Body target, ICollider collision, Direction direction,
            Vector deltaVelocity, double deltaTime)
        {
            var directionVector = direction.Reverse().ToVector();
            double k1 = body.material.Restoring;
            double k2 = target.material.Restoring;
            var force = 
                directionVector 
                * collision.Area 
                * Pow(body.Mass, 1 / 3.0) 
                * Pow(100000000, 2 * (k1 * k2) / (k1 + k2));

            var absDirection = new Vector { x = Abs(directionVector.x), y = Abs(directionVector.y) };
            var absDirectionRotate = new Vector { x = absDirection.y, y = absDirection.x };
            body.Pull(force);

            var Absorption =
                absDirection
                * deltaVelocity
                * body.Mass
                * -Pow(100, Sqrt(body.material.Absorption * target.material.Absorption));

            body.Pull(Absorption);
            
            var Friction = 
                absDirectionRotate 
                * deltaVelocity
                * body.Mass
                * - Pow(1000, (body.material.Friction + target.material.Friction) / 2);

            body.Pull(Friction);

            var Viscosity = 
                deltaVelocity 
                * collision.Area
                * Pow(body.Mass, 1 / 10.0)
                * -Pow(1000000, Max(body.material.Viscosity, target.material.Viscosity));

            body.Pull(Viscosity);

            body.CollisionWith(target, direction);
        }
    }
}