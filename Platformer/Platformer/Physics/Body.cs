using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace Platformer.Physics
{
    class Body : IBody
    {
        public ICollider collider;

        public Vector Force { get; private set; }
        public Vector Velocity { get; private set; }
        private double density = 100;
        

        public object Tag { get; set; }
        public bool MovementRecipient { get; set; }
        public bool MovementEmitter { get; set; }

        public double Mass
        { get { return collider.Volume() * density; } }

        Action<object, Direction> CollisionEvents = (o, d) => { };

        public Body(ICollider collider)
        {
            this.collider = collider;
        }

        public void Pull(Vector vector)
        {
            Force += vector;
        }

        public void Accelerate(Vector vector)
        {
            Velocity += vector;
        }

        public void Move(Vector vector)
        {
            collider.Move(vector);
        }

        public BoxCollider AxisAlignedBoundingBox()
        {
            return collider.AxisAlignedBoundingBox();
        }

        public IRectangle Recrtangle()
        {
            return collider.AxisAlignedBoundingBox();
        }

        public void CollisionWith(IBody body, Direction direction)
        {
            CollisionEvents(body.Tag, direction);
        }

        public Vector Center()
        {
            return collider.Center();
        }

        public void MoveTo(Vector vector)
        {
            var box = Recrtangle(); 
            Vector location = new Vector { x = box.X, y = box.Y };
            Move(vector - location);
        }

        public void Tick(double deltaTime)
        {
            if (!MovementRecipient)
            {
                Force = Vector.Zero();
                Velocity = Vector.Zero();
                return;
            }

            Accelerate(Velocity * -1 / 1000);
            Accelerate(Force * deltaTime / Mass);
            Move(Velocity * deltaTime);
            
            Force = Vector.Zero();
        }
        
        public void AddCollisionEvent(Action<object, Direction> action)
        {
            CollisionEvents += action;
        }

        public void Resize(Vector ratio)
        {
            collider.Resize(ratio);
        }
    }
}
