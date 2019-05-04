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
        double Gravity = 9.8;

        void Tick(double deltaTime)
        {
            for (int i = 0; i < Bodies.Count; i++)
                for (int j = i + 1; j < Bodies.Count; j++)
                {
                    Bodies[i].CollisionWith(Bodies[j]);
                    if (Bodies[i].Movable)
                        Bodies[i].Pull(new Vector {x = 0, y = Gravity });
                }

            foreach (Body body in Bodies)
                body.Tick(deltaTime);
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
