﻿using Platformer.GUI;
using System;
using Platformer.Game;

namespace Platformer.Entities
{
    /// <summary>
    /// Класс, представляющий сущность игрового мира
    /// </summary>
    internal class Entity
    {
        /// <summary>
        /// Приоритет отрисовки
        /// </summary>
        public double DrawPriority { get; set; }

        /// <summary>
        /// Область занимаемая сущностью
        /// </summary>
        public HitBox Hitbox { get; set;  }
        
        /// <summary>
        /// Мир, в котором эта сущность расположена
        /// </summary>
        public World Context { get; set; }

        public string Texture { get; set; }

        /// <summary>
        /// Конструктор, создающий экземпляр сущности по её миру и расположению
        /// </summary>
        /// <param name="context"></param>
        /// <param name="hitbox"></param>
        public Entity(World context, HitBox hitbox)
        {
            Context = context;
            Hitbox = hitbox;
            Texture = "";//new Texture((int)Math.Round(Hitbox.Width), (int)Math.Round(Hitbox.Height));
        }

        /// <summary>
        /// Конструктор, создающий экземпляр сущности по её размеру
        /// (нужен для того, чтобы можно было заготорвить сущность до её непосредственного размещения в том или ином мире)
        /// </summary>
        /// <param name="size"></param>
        public Entity(Vector size)
        {
            Hitbox = new HitBox(0, 0, size.x, size.y);
            Texture = ""; //new Texture((int)Math.Round(Hitbox.Width), (int)Math.Round(Hitbox.Height));
        }

        /// <summary>
        /// Конструктор сущности по умолчанию
        /// </summary>
        public Entity() { }

        /// <summary>
        /// Проверяет, пересекается ли эта сущность с другой
        /// </summary>
        /// <param name="other">Другая сущность</param>
        /// <returns></returns>
        public bool Intersects(Entity other)
            => Hitbox.Intersects(other.Hitbox);

        /// <summary>
        /// Проверяет, пересекается ли эта сущность с заданной оластью
        /// </summary>
        /// <param name="other">Область</param>
        /// <returns></returns>
        public bool Intersects(HitBox other)
            => Hitbox.Intersects(other);

        /// <summary>
        /// Перемещает сущность по направлению вектора
        /// </summary>
        /// <param name="velocity">Вектор скорости</param>
        public void Move(Vector velocity)
            => Hitbox.Move(velocity);

        public virtual void Tick(double deltaTime)
        {
            //Texture.Tick(deltaTime);
        }
    }
}
