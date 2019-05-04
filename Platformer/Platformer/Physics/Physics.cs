using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Physics
{
    class Physics
    {
        List<Body> Bodies = new List<Body>();

        void Tick(double deltaTime)
        {

        }

        public void RemoveBody(Body body)
        {
            Bodies.Remove(body);
        }

        public void Addbody(Body body)
        {
            if (Bodies.Contains(body))
                return;
            Bodies.Add(body);
        }
    }
}
