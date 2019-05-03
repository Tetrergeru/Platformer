using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platformer.Entities;

namespace Platformer
{
    /// <summary>
    /// Геометрический вектор двухмерного пространства
    /// </summary>
    struct Vector
    {
        /// <summary>
        /// Координата X
        /// </summary>
        public double x;

        /// <summary>
        /// Координата Y
        /// </summary>
        public double y;

        /// <summary>
        /// Возвращает копию вектора с обнулённым иксом
        /// </summary>
        /// <returns>Копию вектора с обнулённым иксом</returns>
        public Vector ZeroX()
            => new Vector { x = 0, y = y };

        /// <summary>
        /// Возвращает копию вектора с обнулённым игреком
        /// </summary>
        /// <returns>Копию вектора с обнулённым игреком</returns>
        public Vector ZeroY()
            => new Vector { x = x, y = 0 };

        /// <summary>
        /// Приводит вектор к строковому представлению
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"({x}, {y})";
        }

        /// <summary>
        /// Сумма двух векторов
        /// </summary>
        /// <param name="first">Первое слагаемое</param>
        /// <param name="second">Второе слагаемое</param>
        /// <returns></returns>
        public static Vector operator +(Vector first, Vector second)
            => new Vector
            {
                x = first.x + second.x,
                y = first.y + second.y,
            };

        /// <summary>
        /// Умножает вектор на число
        /// </summary>
        /// <param name="vector">Вектор, который умножаем</param>
        /// <param name="mutiplier">Число, на которое умножаем</param>
        /// <returns></returns>
        public static Vector operator *(Vector vector, double mutiplier)
            => new Vector
            {
                x = vector.x * mutiplier,
                y = vector.y * mutiplier,
            };

        public void SetAxis(Axis axis, double value)
        {
            switch (axis)
            {
                case Axis.Vertical:
                    y = value;
                    return;

                case Axis.Horizontal:
                    x = value;
                    return;

                default:
                    return;
            }
        }

        public bool IsZero()
        {
            return Math.Abs(x) < 1e-10 && Math.Abs(y) < 1e-10;
        }

        /// <summary>
        /// Возвращает нулевой вектор
        /// </summary>
        /// <returns></returns>
        public static Vector Zero()
            => new Vector { x = 0, y = 0 };

        public static Vector InAxis(double distance, Axis axis)
        {
            if (axis == Axis.Horizontal)
                return new Vector{x = distance, y = 0};

            return axis == Axis.Vertical ? new Vector {x = 0, y = distance} : new Vector();
        }
    }
}
