using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Physics
{
    interface IInteraction
    {
        /// <summary>
        /// Производит взаимодействие между собой пары объектов
        /// </summary>
        /// <param name="body1">первый объект</param>
        /// <param name="body2">второй объект</param>
        /// <param name="deltaTime">время последней итерации</param>
        void Collision(Body body1, Body body2, double deltaTime);

        /// <summary>
        /// Производит постоянные взаимодействия на объект, такие как гравитация и сопротивление воздуха
        /// </summary>
        /// <param name="body">объект</param>
        /// <param name="deltaTime">время последней итерации</param>
        void ConstantInteraction(Body body, double deltaTime);
    }
}
