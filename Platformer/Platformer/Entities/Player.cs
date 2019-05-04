namespace Platformer.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// Пероснаж игрока
    /// </summary>
    internal class Player : Actor
    {
        private readonly Game.Game _game;

        /// <inheritdoc />
        /// <summary>
        /// Конструктор, создающий экземпляр игорка по его миру и расположению
        /// </summary>
        /// <param name="context"></param>
        /// <param name="hitbox"></param>
        /// <param name="game"></param>
        public Player(World context, HitBox hitbox, Game.Game game) : base(context, hitbox)
        {
            _game = game;
            MaxHealth = 50;
            Health = MaxHealth;
            JumpHeight = 200 * 9.8;
        }

        /// <inheritdoc />
        /// <summary>
        /// Конструктор, создающий экземпляр игорка по его размеру
        /// (нужен для того, чтобы можно было заготорвить игорка до его непосредственного размещения в мире)
        /// </summary>
        /// <param name="size"></param>
        /// <param name="game"></param>
        public Player(Vector size, Game.Game game) : base(size)
        {
            _game = game;
            MaxHealth = 50;
            Health = MaxHealth;
            JumpHeight = 200 * 9.8;
        }

        /// <inheritdoc />
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
            _game.GameOver();
        }
    }
}
