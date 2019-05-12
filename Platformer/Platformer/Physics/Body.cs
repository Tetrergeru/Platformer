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
        public PhysicalMaterial material { get; set; }
        public bool MovementRecipient { get => material.MovementRecipient; }
        public bool MovementEmitter { get => material.MovementEmitter; }

        public object Tag { get; set; }

        public double Mass => collider.Area * material.Density;

        private Action<object, Direction> CollisionEvents = (o, d) => { };
        private Action<object, Direction> StartCollisionEvents = (o, d) => { };
        private Action<object, Direction> EndCollisionEvents = (o, d) => { };

        private List<(object tag, Direction direction)> LastCollisionList = new List<(object, Direction)>();
        private List<(object tag, Direction direction)> CollisionList = new List<(object, Direction)>();

        public Body(ICollider collider, PhysicalMaterial material)
        {
            this.collider = collider;
            Force = Vector.Zero();
            Velocity = Vector.Zero();
            this.material = material;
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

        public IRectangle AxisAlignedBoundingBox()
        {
            return collider.AxisAlignedBoundingBox();
        }

        public IRectangle Rectangle => collider.AxisAlignedBoundingBox();

        public Vector Center => collider.Center;

        public void MoveTo(Vector vector)
        {
            var box = Rectangle; 

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

        public void SetVelocity(Vector vector)
        {
            Velocity = vector;
        }
    }
}
