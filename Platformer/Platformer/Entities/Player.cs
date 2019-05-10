using System.ComponentModel.Design;
using Platformer.Game;
using Platformer.Physics;

namespace Platformer.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// Пероснаж игрока
    /// </summary>
    internal class Player : Actor
    {
        public override double Health
        {
            get => health;
            internal set { health = 100; }
        }

        /// <inheritdoc />
        /// <summary>
        /// Конструктор, создающий экземпляр игорка по его миру и расположению
        /// </summary>
        /// <param name="context"></param>
        /// <param name="hitbox"></param>
        /// <param name="game"></param>
        public Player(World context, IBody body) : base(context, body)
        {
            MaxHealth = 50;
            Health = MaxHealth;
        }
    }
}
