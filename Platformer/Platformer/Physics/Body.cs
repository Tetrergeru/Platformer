using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

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

        public BoxCollider AxisAlignedBoundingBox()
        {
            return collider.AxisAlignedBoundingBox();
        }

        public void CollisionWith(Body target)
        {
            ICollider collision = collider.CollisionWith(target.collider);
            if (collision is BoxCollider box)
            {
                Vector dist = Center() - target.Center();
                dist.x = Sign(dist.x);
                dist.y = Sign(dist.y);
                Vector force = dist * new Vector { x = box.width, y = box.width };
                Pull(force);
                target.Pull(force * -1);
            }
            else
                throw new NotImplementedException("Неизвестный коллайдер " + collider.GetType().ToString());
        }

        public Vector Center()
        {
            return collider.Center();
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
