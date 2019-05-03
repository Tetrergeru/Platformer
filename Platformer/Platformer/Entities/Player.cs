using Platformer.GUI;

namespace Platformer.Entities
{
    /// <summary>
    /// Пероснаж игрока
    /// </summary>
    class Player : Actor
    {
        private Game game_;

        /// <summary>
        /// Конструктор, создающий экземпляр игорка по его миру и расположению
        /// </summary>
        /// <param name="context"></param>
        /// <param name="hitbox"></param>
        public Player(World context, HitBox hitbox, Game game) : base(context, hitbox)
        {
            game_ = game;
            MaxHealth = 50;
            Health = MaxHealth;
            JumpHeight = 200 * 9.8;
        }

        /// <summary>
        /// Конструктор, создающий экземпляр игорка по его размеру
        /// (нужен для того, чтобы можно было заготорвить игорка до его непосредственного размещения в мире)
        /// </summary>
        /// <param name="size"></param>
        public Player(Vector size, Game game) : base(size)
        {
            game_ = game;
            MaxHealth = 50;
            Health = MaxHealth;
            JumpHeight = 200 * 9.8;
        }

        /// <summary>
        /// Конструктор игрока по умолчанию
        /// </summary>
        public Player()
        {
            MaxHealth = 50;
            Health = MaxHealth;
            JumpHeight = 200 * 9.8;
        }

        public void GameOver()
        {
            game_.GameOver();
        }
    }
}
