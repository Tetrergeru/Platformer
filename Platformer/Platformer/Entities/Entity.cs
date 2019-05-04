﻿using System;
using System.Collections.Generic;
using System.Drawing;
using Platformer.GUI;
using Platformer.Physics;

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

        public Body body;
        
        /// <summary>
        /// Мир, в котором эта сущность расположена
        /// </summary>
        public World Context { get; set; }
        
        public System.Windows.Rect Hitbox()
        {
            var hitbox = body.Recrtangle();
            return new System.Windows.Rect(hitbox.x, hitbox.y, hitbox.width, hitbox.height);
        }

        public ITexture Texture { get; set; }

        /// <summary>
        /// Конструктор, создающий экземпляр сущности по её миру и расположению
        /// </summary>
        /// <param name="context"></param>
        /// <param name="hitbox"></param>
        public Entity(World context, HitBox hitbox)
        {
            Context = context;
            body = new Body(new BoxCollider {x = hitbox.X, y = hitbox.Y, height = hitbox.Height, width = hitbox.Width });
            Texture = new Texture((int)Math.Round(hitbox.Width), (int)Math.Round(hitbox.Height));
        }

        /// <summary>
        /// Конструктор, создающий экземпляр сущности по её размеру
        /// (нужен для того, чтобы можно было заготорвить сущность до её непосредственного размещения в том или ином мире)
        /// </summary>
        /// <param name="size"></param>
        public Entity(Vector size)
        {
            body = new Body(new BoxCollider { x = 0, y = 0, height = size.y, width = size.x});
            var hitbox = body.Recrtangle();
            Texture = new Texture((int)Math.Round(hitbox.width), (int)Math.Round(hitbox.height));
        }

        /// <summary>
        /// Конструктор сущности по умолчанию
        /// </summary>
        public Entity() { }
        
        /// <summary>
        /// Перемещает сущность по направлению вектора
        /// </summary>
        /// <param name="velocity">Вектор скорости</param>
        public void Move(Vector velocity)
            => body.Move(velocity);

        public virtual void Tick(double deltaTime)
        {
            Texture.Tick(deltaTime);
        }
        
        /// <summary>
        /// Перемещает объект в заданные координаты
        /// </summary>
        /// <param name="destination">Место, куда мы перемещаем объект</param>
        public void MoveTo(Vector destination)
        {
            body.MoveTo(destination);
        }

        public static Entity MakeEntity(World context, HitBox hitbox, Bitmap texture, FillType ft, double scale = 1)
        {
            var result = new Entity(context, hitbox);
            result.Texture.AddTexture(texture, ft, scale);
            return result;
        }

        public static Entity MakeEntity(World context, HitBox hitbox, List<Bitmap> texture, FillType ft, double delay,
            double afterDelay, double scale = 1)
        {
            var result = new Entity(context, hitbox)
            {
                Texture = new TextureAnimated((int) hitbox.Width, (int) hitbox.Height, delay, afterDelay)
            };
            foreach (var t in texture)
                result.Texture.AddTexture(t, ft, scale);
            return result;
        }

        public static Entity MakeEntity(World context, HitBox hitbox, ITexture texture)
        {
            var result = new Entity(context, hitbox)
            {
                Texture = texture
            };
            return result;
        }
    }
}