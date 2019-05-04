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
            RunningSpeed = 150;
        }

        /// <inheritdoc />
        /// <summary>
        /// Конструктор, создающий экземпляр монстра по его размеру
        /// (нужен для того, чтобы можно было заготорвить игорка до его непосредственного размещения в мире)
        /// </summary>
        /// <param name="size"></param>
        public Monster(Vector size) : base(size)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Конструктор монстра по умолчанию
        /// </summary>
        public Monster()
        {
        }

        public override void Tick(double deltaTime)
        {/*
            if (hitbox.Centre.DistanceTo(Context.Player.hitbox.Centre) < 300)
            {
                Run(Context.Player.hitbox.Centre.x < hitbox.Centre.x ? Direction.Left : Direction.Right);
                if (Context.Player.hitbox.Centre.y < hitbox.Centre.y)
                {
                    Jump();
                }
            }*/

            base.Tick(deltaTime);
        }
    }
}