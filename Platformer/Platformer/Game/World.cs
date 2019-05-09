using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using Platformer.Entities;
using Platformer.Files;
using Platformer.Physics;

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
        public World()
        {
            _physics = new Physics.Physics();
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                var m = CreateMonster(new HitBox(i * 52 - 700, j * 52 - 1000, 55, 50));
                m.Texture = "Resources/TextureAssets/slime.texture";
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
            var body = _physics.CreateBody(new BoxCollider(hitBox));
            var entity = new Entity(this, body);
            Blocks.Add(entity);
            return entity;
        }

        public Entity CreateDecoration(IRectangle hitBox)
        {
            var body = _physics.CreateBody(new BoxCollider(hitBox));
            var entity = new Entity(this, body);
            Decorations.Add(entity);
            return entity;
        }

        public Monster CreateMonster(IRectangle hitBox)
        {
            var body = _physics.CreateBody(new BoxCollider(hitBox), true);
            var entity = new Monster(this, body);
            Enemies.Add(entity);
            return entity;
        }
        
        public Player CreatePlayer(IRectangle hitBox)
        {
            var body = _physics.CreateBody(new BoxCollider(hitBox), true);
            var entity = new Player(this, body);
            Player = entity;
            return entity;
        }

        public void RemoveEntity(Entity entity)
        {
            if (entity is Monster monster)
                Enemies.Remove(monster);
            if (Blocks.Contains(entity))
                Blocks.Remove(entity);
            Decorations.Remove(entity);
        }

        public void Tick(double deltaTime)
        {
            //Player.Tick(deltaTime);
            _physics.Tick(deltaTime);

            foreach (var x in AllEntities)
                x.Tick(deltaTime);
        }
    }
}
