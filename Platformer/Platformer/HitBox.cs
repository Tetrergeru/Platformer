﻿using System;
using System.Windows;
using System.Drawing;
using Platformer.Entities;

namespace Platformer
{
    /// <summary>
    /// Область, занимаемая игровой сущностью
    /// </summary>
    public class HitBox : IRectangle
    {
        /// <summary>
        /// Сама область в виде прямоугольника
        /// </summary>
        protected Rect rectangle;

        /// <summary>
        /// Координата X хитбокса
        /// </summary>
        public double X => rectangle.X;

        /// <summary>
        /// Координата Y хитбокса
        /// </summary>
        public double Y => rectangle.Y;

        public double X2 => X + Width;

        public double Y2 => Y + Height;

        /// <summary>
        /// Ширина хитбокса
        /// </summary>
        public double Width => rectangle.Width;

        /// <summary>
        /// Высота хитбокса
        /// </summary>
        public double Height => rectangle.Height;

        public Vector Coordinates => new Vector { x = X, y = Y };

        public Vector Centre => new Vector {x = X + Width / 2, y = Y + Height / 2};

        /// <summary>
        /// Конструктор, создающий хитбокс по коррдинатам и размеру
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        /// <param name="w">Ширина</param>
        /// <param name="h">Высота</param>
        public HitBox(double x, double y, double w, double h)
        {
            rectangle = new Rect(x, y, w, h);
        }

        public HitBox(IRectangle hitbox)
        {
            rectangle = new Rect(hitbox.X, hitbox.Y, hitbox.Width, hitbox.Height);
        }

        /// <summary>
        /// Проверяет, пересекается ли хитбокс с другим хитбоксом
        /// </summary>
        /// <param name="other">Второй хитбокс</param>
        /// <returns></returns>
        public bool Intersects(HitBox other)
            => rectangle.IntersectsWith(other.rectangle);
        
        /// <summary>
        /// Смещает объект на вектор скорости
        /// </summary>
        /// <param name="velocity">Скорость перемещения объекта</param>
        public void Move(Vector velocity)
        {
            rectangle.X += velocity.x;
            rectangle.Y += velocity.y;
        }

        /// <summary>
        /// Перемещает объект в заданные координаты
        /// </summary>
        /// <param name="destination">Место, куда мы перемещаем объект</param>
        public void MoveTo(Vector destination)
        {
            rectangle.X = destination.x;
            rectangle.Y = destination.y;
        }
        
        /// <summary>
        /// Приводит хитбюокс к строковому представлению
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
