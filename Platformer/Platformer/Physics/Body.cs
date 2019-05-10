using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
using static System.ValueType;

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
        Action<object, Direction> StartCollisionEvents = (o, d) => { };
        Action<object, Direction> EndCollisionEvents = (o, d) => { };
        
        List<(object tag, Direction direction)> LastCollisionList = new List<(object, Direction)>();
        List<(object tag, Direction direction)> CollisionList = new List<(object, Direction)>();

        public Body(ICollider collider)
        {
            this.collider = collider;
            Force = Vector.Zero();
            Velocity = Vector.Zero();
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

            Pull(Velocity * -100000);
            Accelerate(Force * deltaTime / Mass);
            Move(Velocity * deltaTime);

            foreach (var t in LastCollisionList)
                EndCollisionEvents(t.tag, t.direction);
            LastCollisionList = CollisionList;
            CollisionList = new List<(object, Direction direction)>();

            Force = Vector.Zero();
        }
        
        public void CollisionWith(IBody body, Direction direction)
        {
            CollisionEvents(body.Tag, direction);
            CollisionList.Add((body.Tag, direction));

            if (LastCollisionList.Contains((body.Tag, direction)))
                LastCollisionList.Remove((body.Tag, direction));
            else
                StartCollisionEvents(body.Tag, direction);
        }

        public void Resize(Vector ratio)
        {
            collider.Resize(ratio);
        }

        public void AddCollisionEvent(Action<object, Direction> action)
        {
            CollisionEvents += action;
        }
        
        public void AddStartCollisionEvent(Action<object, Direction> action)
        {
            StartCollisionEvents += action;
        }

        public void AddEndCollisionEvent(Action<object, Direction> action)
        {
            EndCollisionEvents += action;
        }
    }
}
