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
        double Gravity = 9.8;

        public void Tick(double deltaTime)
        {
            for (int i = 0; i < Bodies.Count; i++)
            {
                for (int j = i + 1; j < Bodies.Count; j++)
                    Collision.Interaction(Bodies[i], Bodies[j], deltaTime);
                
                if (Bodies[i].MovementRecipient)
                    Bodies[i].Pull(new Vector { x = 0, y = Gravity } * Bodies[i].Mass);
            }
            //System.Threading.Thread.Sleep(10);
            foreach (Body body in Bodies)
                body.Tick(deltaTime);
        }

        public IBody CreateBody(ICollider collider, PhysicalMaterial material)
        {
            Body body = new Body(collider, material);
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
