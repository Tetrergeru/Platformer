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
        public List<Entity> Blocks { get; } = new List<Entity>();

        public List<Entity> Decorations { get; } = new List<Entity>();

        public IEnumerable<Entity> AllEntities
        {
            get
            {
                yield return Player;
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
                foreach (var x in Blocks)
                    yield return x;
            }
        }

        /// <summary>
        /// Инициализирует мир значениями по умолчанию
        /// </summary>
        public World()
        {
            Gravity = new Vector {x = 0, y = 1000 * 9.8};
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

            foreach (var x in Decorations)
                x.Tick(deltaTime);
        }
    }
}
