namespace Platformer.Entities
{
    /// <summary>
    /// Пероснаж игрока
    /// </summary>
    class Player : Actor
    {
        /// <summary>
        /// Конструктор, создающий экземпляр игорка по его миру и расположению
        /// </summary>
        /// <param name="context"></param>
        /// <param name="hitbox"></param>
        public Player(World context, HitBox hitbox) : base(context, hitbox)
        {
            JumpHeight = 200 * 9.8;
        }

        /// <summary>
        /// Конструктор, создающий экземпляр игорка по его размеру
        /// (нужен для того, чтобы можно было заготорвить игорка до его непосредственного размещения в мире)
        /// </summary>
        /// <param name="size"></param>
        public Player(Vector size) : base(size)
        {
            JumpHeight = 200 * 9.8;
            RunningSpeed = 200;
        }


        /// <summary>
        /// Конструктор игрока по умолчанию
        /// </summary>
        public Player()
        {
            JumpHeight = 200 * 9.8;
        }
    }
}
