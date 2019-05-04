namespace Platformer.Entities
{
    internal class Monster : Actor
    {
        /// <inheritdoc />
        /// <summary>
        /// Конструктор, создающий экземпляр монстра по его миру и расположению
        /// </summary>
        /// <param name="context"></param>
        /// <param name="hitbox"></param>
        public Monster(World context, HitBox hitbox) : base(context, hitbox)
        {
            MaxHealth = 50;
            Health = MaxHealth;
            jumpHeight = 150 * 9.8;
            runningSpeed = 150;
        }

        /// <inheritdoc />
        /// <summary>
        /// Конструктор, создающий экземпляр монстра по его размеру
        /// (нужен для того, чтобы можно было заготорвить игорка до его непосредственного размещения в мире)
        /// </summary>
        /// <param name="size"></param>
        public Monster(Vector size) : base(size)
        {
            jumpHeight = 100 * 9.8;
        }

        /// <inheritdoc />
        /// <summary>
        /// Конструктор монстра по умолчанию
        /// </summary>
        public Monster()
        {
            jumpHeight = 100 * 9.8;
        }

        public override void Tick(double deltaTime)
        {
            if (Hitbox.Centre.DistanceTo(Context.Player.Hitbox.Centre) < 300)
            {
                Run(Context.Player.Hitbox.Centre.x < Hitbox.Centre.x ? Direction.Left : Direction.Right);
                if (Context.Player.Hitbox.Centre.y < Hitbox.Centre.y)
                {
                    Jump();
                }
            }

            base.Tick(deltaTime);
        }
    }
}