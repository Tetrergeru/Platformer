using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Physics
{
    interface IPhysics
    {
        /// <summary>
        /// Создаёт новое физическое тело внутри движка
        /// </summary>
        /// <param name="collider">коллайдер</param>
        /// <param name="MovementRecipient">возможность физики управлять движением телои</param>
        /// <param name="MovementEmitter">Возможность тела влиять на физическое движение</param>
        /// <returns></returns>
        IBody CreateBody(ICollider collider, bool MovementRecipient = false, bool MovementEmitter = false);

        /// <summary>
        /// Удаляет физическое тело из пространства движка
        /// </summary>
        /// <param name="body">удаляемое тело</param>
        /// <returns>false если тело уже удалено</returns>
        bool RemoveBody(IBody body);

        /// <summary>
        /// Такт движка
        /// </summary>
        /// <param name="deltaTime">время, прошедшее с последнего вызова</param>
        void Tick(double deltaTime);
    }
}
