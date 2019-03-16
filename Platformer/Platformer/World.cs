using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using Platformer.Files;

namespace Platformer
{
    /// <summary>
    /// Представляет собой игровой мир
    /// </summary>
    class World
    {
        public Vector Gravity { get; }

        /// <summary>
        /// Ссылка на игрока
        /// </summary>
        public Player player;

        /// <summary>
        /// Твёрдые объекты, структура мира...
        /// </summary>
        public List<Entity> block;

        public List<Entity> Background;

        public List<Entity> Frontground;

        /// <summary>
        /// Возвращает все объекты, с которыми можно взаимойдействовать
        /// </summary>
        public IEnumerable<Entity> Entities
        {
            get
            {
                yield return player;
                foreach (var x in block)
                    yield return x;
            }
        }

        /// <summary>
        /// Инициализирует мир значениями по умолчанию
        /// </summary>
        public World()
        {
            Gravity = new Vector {x = 0, y = 1000 * 9.8};
            block = new List<Entity>()
            {
                Entity.MakeEntity(this, new HitBox(0, 400, 300, 100),
                    new Bitmap("Resources/Textures/Grass_1.png"),FillType.StretchDown),
                Entity.MakeEntity(this, new HitBox(700, 400, 300, 100),
                    new Bitmap("Resources/Textures/Grass_1.png"),FillType.StretchDown),
                Entity.MakeEntity(this, new HitBox(300, 400, 400, 100),
                    new Bitmap("Resources/Textures/Stone_1.png"),FillType.StretchDown),
                Entity.MakeEntity(this, new HitBox(280, 350, 440, 50),
                    new Bitmap("Resources/Textures/Grass_1.png"),FillType.Repeat, 10),
            };

            Background = new List<Entity>
            {
                Entity.MakeEntity(this, new HitBox(350, 50, 300, 300), TextureFile.GetTexture("Resources/TextureAssets/Tree_0.texture")),
            };

            TextureAnimated texture = new TextureAnimated(120, 40, 1, 0);
            texture.AddTexture(new Bitmap("Resources/Textures/HighGrass_1.png"), FillType.Stretch);
            texture.AddTexture(new Bitmap("Resources/Textures/HighGrass_0.png"), FillType.Stretch);

            Frontground = new List<Entity>()
            {
                Entity.MakeEntity(this, new HitBox(280, 310, 120, 40), texture),
                Entity.MakeEntity(this, new HitBox(395, 310, 120, 40), texture),
                Entity.MakeEntity(this, new HitBox(510, 310, 120, 40), texture),
                Entity.MakeEntity(this, new HitBox(605, 310, 120, 40), texture),
            };
        }

        /// <summary>
        /// Устанавливает связь игрока с миром
        /// </summary>
        /// <param name="player"></param>
        /// <param name="coords"></param>
        public void SetPlayer(Player player, Vector coords)
        {
            this.player = player;
            player.Context = this;
            player.Hitbox.MoveTo(coords);
        }

        public void Tick(double deltaTime)
        {
            player.Tick(deltaTime);

            foreach (var x in Background)
                x.Tick(deltaTime);

            foreach (var x in Frontground)
                x.Tick(deltaTime);
        }
    }
}
