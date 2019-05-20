using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Platformer.Physics.Physics
{
    internal class HashPhysics : IPhysics
    {
        private ChunkTable<Body> _bodies = new ChunkTable<Body>(b => b.Rectangle, (2, 2));
        IInteraction _interaction;

        public HashPhysics(IInteraction interaction)
        {
            _interaction = interaction;
        }

        public void Tick(double deltaTime)
        {
            _bodies.Refresh();
            foreach (var pair in _bodies.Collisions())
                _interaction.Collision(pair.Item1, pair.Item2, deltaTime);

            foreach (Body body in _bodies)
            {
                _interaction.ConstantInteraction(body, deltaTime);
                body.Tick(deltaTime);
            }
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
