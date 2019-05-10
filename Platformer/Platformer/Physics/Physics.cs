using System.Collections.Generic;

namespace Platformer.Physics
{
    internal class Physics : IPhysics
    {
        private readonly List<Body> _bodies = new List<Body>();

        private readonly double _gravity;

        public Physics(double gravity = 9.8)
        {
            _gravity = gravity;
        }

        public void Tick(double deltaTime)
        {
            for (var i = 0; i < _bodies.Count; i++)
            {
                for (var j = i + 1; j < _bodies.Count; j++)
                    Collision.Interaction(_bodies[i], _bodies[j], deltaTime);
                
                if (_bodies[i].MovementRecipient)
                    _bodies[i].Pull(new Vector { x = 0, y = _gravity } * _bodies[i].Mass);
            }
            //System.Threading.Thread.Sleep(10);
            foreach (var body in _bodies)
                body.Tick(deltaTime);
        }

        public IBody CreateBody(ICollider collider, PhysicalMaterial material)
        {
            var body = new Body(collider, material);
            _bodies.Add(body);
            return body;
        }

        public bool RemoveBody(IBody body)
        {
            if (body is Body b)
                return _bodies.Remove(b);
            return false;
        }
    }
}
