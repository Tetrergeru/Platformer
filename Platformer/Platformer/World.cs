using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    /// <summary>
    /// Представляет собой игровой мир
    /// </summary>
    class World
    {
        /// <summary>
        /// Ссылка на игрока
        /// </summary>
        public Player player;

        /// <summary>
        /// Твёрдые объекты, структура мира...
        /// </summary>
        public SolidBlock[] block;

        public Entity[] Background;

        public Entity[] Frontground;

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
            block = new []
            {
                SolidBlock.Make(this, new HitBox(0, 400, 300, 100),
                    new Bitmap("Resources/Textures/Grass_1.png"),FillType.StretchDown),
                SolidBlock.Make(this, new HitBox(700, 400, 300, 100),
                    new Bitmap("Resources/Textures/Grass_1.png"),FillType.StretchDown),
                SolidBlock.Make(this, new HitBox(300, 400, 400, 100),
                    new Bitmap("Resources/Textures/Stone_1.png"),FillType.StretchDown),
                SolidBlock.Make(this, new HitBox(280, 350, 440, 50),
                    new Bitmap("Resources/Textures/Grass_1.png"),FillType.Repeat, 10),
            };

            Background = new[]
            {
                Entity.Make(this, new HitBox(350, 50, 300, 300),
                    new Bitmap("Resources/Textures/Tree_1.png"), FillType.Stretch)
            };

            Frontground = new[]
            {
                Entity.Make(this, new HitBox(350, 50, 300, 300),
                    new Bitmap("Resources/Textures/Tree_1.png"), FillType.Stretch)
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
    }
}
