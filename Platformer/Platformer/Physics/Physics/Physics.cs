using System.Collections.Generic;
using System;

namespace Platformer.Physics.Physics
{
    internal class Physics : IPhysics
    {
        private readonly List<Body> _bodies = new List<Body>();
        IInteraction _interaction;

        public Physics(IInteraction interaction)
        {
            _interaction = interaction;
        }

        public void Tick(double deltaTime)
        {
            for (var i = 0; i < _bodies.Count; i++)
            {
                for (var j = i + 1; j < _bodies.Count; j++)
                    _interaction.Collision(_bodies[i], _bodies[j], deltaTime);

                if (_bodies[i].MovementRecipient)
                    _interaction.ConstantInteraction(_bodies[i], deltaTime);
            }

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
