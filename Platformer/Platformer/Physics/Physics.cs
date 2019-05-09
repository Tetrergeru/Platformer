using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Physics
{
    class Physics : IPhysics
    {
        List<Body> Bodies = new List<Body>();
        double Gravity = 9.8 * 100;

        public void Tick(double deltaTime)
        {
            for (int i = 0; i < Bodies.Count; i++)
            {
                for (int j = i + 1; j < Bodies.Count; j++)
                    Bodies[i].CollisionWith(Bodies[j]);
               
                if (Bodies[i].Movable)
                    Bodies[i].Pull(new Vector { x = 0, y = Gravity } * Bodies[i].Mass);
            }

            foreach (Body body in Bodies)
                body.Tick(deltaTime);
        }

        public IBody CreateBody(ICollider collider, bool movable = false)
        {
            Body body = new Body(collider, movable);
            Bodies.Add(body);
            return body;
        }

        public bool RemoveBody(IBody body)
        {
            if (body is Body b)
                return Bodies.Remove(b);
            return false;
        }
    }
}
