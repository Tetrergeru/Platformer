using System.Collections.Generic;
using System.Drawing;
using Platformer.Files;

namespace Platformer
{
    /// <summary>
    /// Представляет собой игровой мир
    /// </summary>
    internal class World
    {
        public Color BackGroundColor { get; set; }

        public Vector Gravity { get; }

        /// <summary>
        /// Ссылка на игрока
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>
        /// Твёрдые объекты, структура мира...
        /// </summary>
        public List<Entity> Block { get; } = new List<Entity>();

        public List<Entity> Background { get; } = new List<Entity>();

        public List<Entity> Frontground { get; } = new List<Entity>();

        /// <summary>
        /// Возвращает все объекты, с которыми можно взаимойдействовать
        /// </summary>
        public IEnumerable<Entity> Entities
        {
            get
            {
                yield return Player;
                foreach (var x in Block)
                    yield return x;
            }
        }

        /// <summary>
        /// Инициализирует мир значениями по умолчанию
        /// </summary>
        public World()
        {
            Gravity = new Vector {x = 0, y = 1000 * 9.8};
            /*
            Block = new List<Entity>()
            {
                Entity.MakeEntity(this, new HitBox(0, 400, 300, 100),
                    TextureFile.GetTexture("Resources/TextureAssets/grass_0.texture",300, 100)),
                Entity.MakeEntity(this, new HitBox(700, 400, 300, 100),
                    TextureFile.GetTexture("Resources/TextureAssets/grass_0.texture",300, 100)),
                Entity.MakeEntity(this, new HitBox(300, 400, 400, 100),
                    TextureFile.GetTexture("Resources/TextureAssets/stone_0.texture",400, 100)),
                Entity.MakeEntity(this, new HitBox(280, 350, 440, 50),
                    TextureFile.GetTexture("Resources/TextureAssets/grass_0.texture",440, 50)),
            };

            Background = new List<Entity>
            {
                Entity.MakeEntity(this, new HitBox(350, 50, 300, 300), TextureFile.GetTexture("Resources/TextureAssets/Tree_0.texture", 50, 50)),
            };
            
            Frontground = new List<Entity>()
            {
                Entity.MakeEntity(this, new HitBox(280, 310, 440, 40), TextureFile.GetTexture("Resources/TextureAssets/highgrass_0.texture", 440, 40)),
            };
            //*/
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
            player.Hitbox.MoveTo(coords);
        }

        public void Tick(double deltaTime)
        {
            Player.Tick(deltaTime);

            foreach (var x in Background)
                x.Tick(deltaTime);

            foreach (var x in Frontground)
                x.Tick(deltaTime);
        }
    }
}
