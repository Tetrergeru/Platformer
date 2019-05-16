using System;
using static System.Math;

namespace Platformer.Physics
{
    internal class Interaction : IInteraction
    {
        private double _gravity;
        private double _airResistance;

        public Interaction(double gravity = 9.8, double airResistance = 0.0003)
        {
            _gravity = gravity;
            _airResistance = airResistance;
        }

        public void Collision(Body body1, Body body2, double deltaTime)
        {
            if (!body1.MovementRecipient && !body2.MovementRecipient)
                return;
            if (!body1.MovementEmitter && !body2.MovementEmitter)
                return;

            var collision = body1.collider.CollisionWith(body2.collider);
            if (collision is BoxCollider box)
            {
                if (box.Volume < 1e-10)
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
                    CollisionWith(body1, body2, box, direction, deltaVelocity, deltaTime);

                if (body2.MovementRecipient && body1.MovementEmitter)
                    CollisionWith(body2, body1, box, direction.Reverse(), deltaVelocity * -1, deltaTime);
            }
            else
            {
                throw new NotImplementedException("Неизвестный коллайдер " + collision.GetType());
            }
        }

        private static void CollisionWith(Body body, Body target, ICollider collision, Direction direction,
            Vector deltaVelocity, double deltaTime)
        {
            double k1 = body.material.Restoring;
            double k2 = target.material.Restoring;
            Vector directionVector = direction.Reverse().ToVector();
            Vector force = 
                directionVector 
                * collision.Volume 
                * Pow(body.Mass, 1 / 3.0) 
                * Pow(100000000, 2 * (k1 * k2) / (k1 + k2));

            body.Pull(force);

            Vector absDirection = new Vector { x = Abs(directionVector.x), y = Abs(directionVector.y) };
            Vector Absorption =
                absDirection
                * deltaVelocity
                * body.Mass
                * -Pow(100, Sqrt(body.material.Absorption * target.material.Absorption));

            body.Pull(Absorption);

            Vector absDirectionRotate = new Vector { x = absDirection.y, y = absDirection.x };
            Vector Friction = 
                absDirectionRotate 
                * deltaVelocity
                * body.Mass
                * - Pow(1000, (body.material.Friction + target.material.Friction) / 2);

            body.Pull(Friction);

            Vector Viscosity = 
                deltaVelocity 
                * collision.Volume
                * Pow(body.Mass, 1 / 10.0)
                * -Pow(1000000, Max(body.material.Viscosity, target.material.Viscosity));

            body.Pull(Viscosity);

            if(body.material.Fluidity)
                body.CompressionForce += absDirection * collision.Volume;
                
            body.CollisionWith(target, direction);
        }

        public void ConstantInteraction(Body body, double deltaTime)
        {
            if (!body.MovementRecipient)
            {
                body.Force = Vector.Zero();
                body.Velocity = Vector.Zero();
                body.CompressionForce = Vector.Zero();
                body.CompressionVelocity = Vector.Zero();
                return;
            }

            body.Pull(new Vector { x = 0, y = _gravity } * body.Mass);
            body.Pull(body.Velocity  * - _airResistance);

            body.Accelerate(body.Force * deltaTime / body.Mass);
            body.Move(body.Velocity * deltaTime);
            body.Force = Vector.Zero();

            if (body.material.Fluidity)
            {
                body.CompressionVelocity += body.CompressionForce;

                double k = body.CompressionVelocity.x / body.CompressionVelocity.y;

                if (k > 100) k = 100;
                if (1 / k > 100) k = 1 / 100.0;

                body.collider.VolumetricResize(k);

                body.CompressionVelocity = 
                    body.CompressionVelocity 
                    * Pow(0.001, 
                        deltaTime 
                        * Pow(10000000, body.material.Ductility) 
                        / 100000);

                Vector vol = body.DefCompression;
                double surfaceTension = Pow(0.00001, 1 - body.material.SurfaceTension);
                body.CompressionForce = new Vector { x = vol.y * surfaceTension, y = vol.x * surfaceTension };
            }
        }
    }
}