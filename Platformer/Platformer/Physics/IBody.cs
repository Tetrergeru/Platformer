using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Physics
{
    interface IBody
    {
        /// <summary>
        /// Определяет, упрявляется ли перемещение данного тела физическим движком
        /// </summary>
        bool Movable { get; set; }

        /// <summary>
        /// Воздействует заданной силой на тело
        /// </summary>
        /// <param name="vector">сила</param>
        void Pull(Vector vector);

        /// <summary>
        /// Меняет скорость тела на заданную величину
        /// </summary>
        /// <param name="vector">значение изменения скорости</param>
        void Accelerate(Vector vector);

        /// <summary>
        /// Изменяет расположение тела на заданное расстояние
        /// </summary>
        /// <param name="vector">изменение расстояния</param>
        void Move(Vector vector);

        /// <summary>
        /// Перемещяет тело в заданную точку
        /// </summary>
        /// <param name="vector">коодината точки</param>
        void MoveTo(Vector vector);
        
        /// <summary>
        /// Возвращает прямоугольную область, занимаемую данным телом
        /// </summary>
        /// <returns></returns>
        BoxCollider Recrtangle();
    }
}
