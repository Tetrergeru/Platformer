using System;
using System.Collections.Generic;
using System.Linq;
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
        public Entity[] block; 

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
            block = new Entity[]
            {
                new Entity(this, new HitBox(0, 400, 1000, 40)),
                new Entity(this, new HitBox(0, 40, 10, 400)),
                new Entity(this, new HitBox(800, 40, 10, 400)),
                new Entity(this, new HitBox(600, 250, 50, 10)),
                new Entity(this, new HitBox(500, 300, 50, 10)),
                new Entity(this, new HitBox(0, -10, 1000, 20)),
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
