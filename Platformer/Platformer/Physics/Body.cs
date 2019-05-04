using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Physics
{
    class Body
    {
        ICollider collider;

        Vector force = Vector.Zero();
        Vector velocity = Vector.Zero();
        double density = 1;
        public bool Movable { get; set; }

        public Body(ICollider collider, bool movable = false)
        {
            this.collider = collider;
            Movable = movable;
        }

        public double Mass
        { get { return collider.Volume() * density; } }

        public void Pull(Vector vector)
        {
            force += vector;
        }

        public void Accelerate(Vector vector)
        {
            velocity += vector;
        }

        public void Move(Vector vector)
        {
            collider.Move(vector);
        }

        public void CollisionWith(Body body)
        {
            throw new Exception();
        }

        public void Tick(double deltaTime)
        {
            if (!Movable)
            {
                force = Vector.Zero();
                return;
            }
            Accelerate(force* deltaTime / Mass);
            Move(velocity * deltaTime);

            force = Vector.Zero();
        }
    }
}
