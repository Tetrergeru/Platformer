using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Physics.Physics
{
    class SortedPhysics : IPhysics
    {
        private readonly List<Body> _bodies = new List<Body>();
        IInteraction _interaction;

        public SortedPhysics(IInteraction interaction)
        {
            _interaction = interaction;
        }

        public void Tick(double deltaTime)
        {
            for (int i = 0; i < _bodies.Count - 1; i++)
                for (int j = i + 1; j >= 1 && _bodies[j - 1].AxisAlignedBoundingBox().X > _bodies[j].AxisAlignedBoundingBox().X; j--)
                    SwapBodies(j - 1, j);

            for (int i = 0; i < _bodies.Count - 1; i++)
                for (int j = i + 1; j < _bodies.Count && _bodies[i].AxisAlignedBoundingBox().X + _bodies[i].AxisAlignedBoundingBox().Width > _bodies[j].AxisAlignedBoundingBox().X; j++)
                    _interaction.Collision(_bodies[i], _bodies[j], deltaTime);

            foreach (Body body in _bodies)
            {
                _interaction.ConstantInteraction(body, deltaTime);
                body.Tick(deltaTime);
            }
        }
        private void SwapBodies(int index1, int index2)
        {
            Body buf = _bodies[index1];
            _bodies[index1] = _bodies[index2];
            _bodies[index2] = buf;
        }

        public IBody CreateBody(ICollider collider, PhysicalMaterial material)
        {
            Body body = new Body(collider, material);
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
