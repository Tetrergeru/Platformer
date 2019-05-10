using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platformer.Game;
using Platformer.Physics;

namespace Platformer.Entities
{
    internal class Particle : Entity
    {
        public double Lifetime { get; set; }

        public Particle(World context, IBody body) :base(context, body)
        {
            Lifetime = 1;
        }

        public void Pull(Vector force)
        {
            _body.Pull(force);
        }

        public override void Tick(double deltaTime)
        {
            base.Tick(deltaTime);
            Lifetime -= deltaTime;
        }
    }
}
