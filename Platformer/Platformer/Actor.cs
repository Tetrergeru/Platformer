using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class Actor : Entity
    {
        protected Vector impuls;

        public void Push(Vector impuls)
        {
            this.impuls += impuls;
        }

        public void Move()
        {

        }
    }
}
