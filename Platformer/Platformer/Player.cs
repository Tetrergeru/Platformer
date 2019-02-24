using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class Player : Actor
    {
        public Player(World context, HitBox hitbox) : base(context, hitbox) { }
    }
}
