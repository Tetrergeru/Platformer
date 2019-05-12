﻿using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using Platformer.Entities;
using Platformer.Files;
using Platformer.Physics;
using System.Linq;
using System;

namespace Platformer.Game
{
    /// <summary>
    /// Представляет собой игровой мир
    /// </summary>
    internal class World
    {
        //public Color BackGroundColor { get; set; }

        private IPhysics _physics;

        /// <summary>
        /// Ссылка на игрока
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>
        /// Твёрдые объекты, структура мира...
        /// </summary>
        private List<Entity> Blocks { get; } = new List<Entity>();

        private List<Entity> Decorations { get; } = new List<Entity>();

        private HashSet<Actor> Enemies { get; } = new HashSet<Actor>();

        private List<Particle> Particles { get; } = new List<Particle>();

        public IEnumerable<Entity> AllEntities
        {
            get
            {
                yield return Player;
                foreach (var m in Enemies)
                    yield return m;
                foreach (var b in Blocks)
                    yield return b;
                foreach (var d in Decorations)
                    yield return d;
                foreach (var d in Particles)
                    yield return d;
            }
        }

        /// <summary>
        /// Возвращает все объекты, с которыми можно взаимойдействовать
        /// </summary>
        public IEnumerable<Entity> SolidEntities
        {
            get
            {
                yield return Player;
                foreach (var x in Enemies)
                    yield return x;
                foreach (var x in Blocks)
                    yield return x;
            }
        }

        /// <summary>
        /// Инициализирует мир значениями по умолчанию
        /// </summary>
        public World(IPhysics physics)
        {
            _physics = physics;

            Random rnd = new Random();

            for (int i = -10; i < 20; i++)
                for (int j = -10; j < 20; j++)
                {
                    var m = CreateParticle(new HitBox(i * 0.5 + rnd.NextDouble(), j * 0.52 + rnd.NextDouble() - 7, 0.005, 0.005));
                    m.Lifetime = 65536;
                    m.Texture = "Resources/TextureAssets/snowflake.texture";
                }
        }

        /// <summary>
        /// Устанавливает связь игрока с миром
        /// </summary>
        /// <param name="player"></param>
        /// <param name="coords"></param>
        public void SetPlayer(Player player, Vector coords)
        {
            Player = player;
            player.Context = this;
            player.MoveTo(coords);
        }

        public Entity CreateSolidEntity(IRectangle hitBox)
        {
            var body = _physics.CreateBody(new BoxCollider(hitBox),
                new PhysicalMaterial
                {
                    Absorption = 0.8,
                    Restoring = 0.8,
                    Viscosity = 0.3,
                    Density = 770,
                    Friction = 0.2,
                    MovementEmitter = true,
                    MovementRecipient = false,
                });
            var entity = new Entity(this, body);
            Blocks.Add(entity);
            return entity;
        }

        public Entity CreateDecoration(IRectangle hitBox)
        {
            var body = _physics.CreateBody(new BoxCollider(hitBox),
                new PhysicalMaterial
                {
                    Absorption = 0.1,
                    Restoring = 0.5,
                    Viscosity = 0.1,
                    Density = 690,
                    Friction = 0.1,
                    MovementEmitter = false,
                    MovementRecipient = false,
                });
            var entity = new Entity(this, body);
            Decorations.Add(entity);
            return entity;
        }

        public Monster CreateMonster(IRectangle hitBox)
        {
            var body = _physics.CreateBody(new BoxCollider(hitBox),
                new PhysicalMaterial
                {
                    Absorption = 0.1,
                    Restoring = 0.5,
                    Viscosity = 0.7,
                    Density = 1000,
                    Friction = 0.7,
                    MovementEmitter = true,
                    MovementRecipient = true,
                });
            var entity = new Monster(this, body);
            Enemies.Add(entity);
            return entity;
        }

        public Particle CreateParticle(IRectangle hitBox)
        {
            var body = _physics.CreateBody(new BoxCollider(hitBox),
                new PhysicalMaterial
                {
                    Absorption = 0.9,
                    Restoring = 0.8,
                    Viscosity = 0.0,
                    Density = 100,
                    Friction = 0.9,
                    MovementEmitter = false,
                    MovementRecipient = true,
                });
            var entity = new Particle(this, body);
            Particles.Add(entity);
            return entity;
        }

        public Player CreatePlayer(IRectangle hitBox)
        {
            var body = _physics.CreateBody(new BoxCollider(hitBox),
                new PhysicalMaterial
                {
                    Absorption = 0.9,
                    Restoring = 0.8,
                    Viscosity = 0.0,
                    Density = 2200,
                    Friction = 0.1,
                    MovementEmitter = true,
                    MovementRecipient = true,
                });
            var entity = new Player(this, body);
            Player = entity;
            return entity;
        }

        public void RemoveBody(IBody body)
        {
            _physics.RemoveBody(body);
        }

        public void RemoveEntity(Entity entity)
        {
            entity.RemoveBody();

            if (entity is Monster monster)
                Enemies.Remove(monster);
            if (Blocks.Contains(entity))
                Blocks.Remove(entity);
            if (Particles.Contains(entity))
                Particles.Remove(entity as Particle);
            Decorations.Remove(entity);
        }

        public void RemoveMonstor(Entity entity)
        {
            int count = 10;
            Random rnd = new Random();
            Vector size = new Vector { x = entity.Hitbox.Width / count, y = entity.Hitbox.Height / count };
            if (size.x > 0.001 && size.y > 0.001)
            {
                double vol = size.x * size.y;
                for (int i = 0; i < count; i++)
                    for (int j = 0; j < count; j++)
                    {
                        var p = CreateParticle(new HitBox(entity.Hitbox.X + size.x * i, entity.Hitbox.Y + size.y * j, size.x, size.y));
                        p.Texture = "Resources/TextureAssets/slime.texture";
                        p._body.Accelerate(new Vector { x = ((i - count / 2) * 100 / count + rnd.Next(-100, 100)) * vol, y = ((j / count) * 100 / count + rnd.Next(-100, 100)) * vol });
                    }
            }
            RemoveEntity(entity);
        }

        public void Tick(double deltaTime)
        {
            //Player.Tick(deltaTime);

            foreach (var x in AllEntities)
                x.Tick(deltaTime);

            _physics.Tick(deltaTime);

            var buf1 = Enemies.Where(e => e.Health <= 0).ToList();
            foreach (var e in buf1)
                RemoveMonstor(e);
            var buf2 = Particles.Where(e => e.Lifetime <= 0).ToList();
            foreach (var e in buf2)
                RemoveEntity(e);
        }
    }
}
