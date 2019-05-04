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
        double slowdown;
        private double density = 1;
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

        public BoxCollider Recrtangle()
        {
            return collider.AxisAlignedBoundingBox();
        }

        public void CollisionWith(Body target)
        {
            if (!Movable && !target.Movable)
                return;
            ICollider collision = collider.CollisionWith(target.collider);
            if (collision is BoxCollider box)
            {
                if (box.Volume() < 0.000001)
                    return;
                Vector dist = Center() - target.Center();
                dist.x = Sign(dist.x);
                dist.y = Sign(dist.y);
                Vector force;
                if(box.width > box.height)
                    force = dist * new Vector { x = 0, y = box.height } * 10000000;
                else
                    force = dist * new Vector { x = box.width, y = 0 } * 10000000;

                slowdown += box.Volume() / collider.Volume();
                target.slowdown += box.Volume() / target.collider.Volume();

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

        public void MoveTo(Vector vector)
        {
            var box = Recrtangle(); 
            Vector location = new Vector { x = box.x, y = box.y };
            Move(vector - location);
        }

        public void Tick(double deltaTime)
        {
            if (!Movable)
            {
                slowdown = 0;
                force = Vector.Zero();
                return;
            }
            
            Accelerate(force * deltaTime / Mass);
            double k = 1 - (slowdown + 0.0001);
            if (k < 0)
                velocity = Vector.Zero();
            else
                velocity = velocity * Pow(k , deltaTime * 100);
            Move(velocity * deltaTime);

            slowdown = 0;
            force = Vector.Zero();
        }
    }
}
