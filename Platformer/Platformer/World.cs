using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class World
    {
        public Player player;

        public Entity[] block; 

        public IEnumerable<Entity> Entities
        {
            get
            {
                yield return player;
                foreach (var x in block)
                    yield return x;
            }
        }

        public World()
        {
            player = new Player(this, new HitBox(100, 0, 20, 40));
            block = new Entity[]
            {
                new Entity(this, new HitBox(0, 400, 1000, 40)),
                new Entity(this, new HitBox(0, 40, 10, 400)),
                new Entity(this, new HitBox(800, 40, 10, 400)),
            };
        }
    }
}
