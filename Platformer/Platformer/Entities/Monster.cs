using Platformer.Game;
using Platformer.Physics;

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
        public Monster(World context, IBody body) : base(context, body)
        {
            MaxHealth = 50;
            Health = MaxHealth;
            jumpHeight = 30;
        }

        public override void Tick(double deltaTime)
        {
            
            if (Hitbox.Coordinates.DistanceTo(Context.Player.Hitbox.Coordinates) < 500000)
            {
                if (Context.Player.Hitbox.X < Hitbox.X)
                    RunLeft();
                else
                    RunRight();

                if (Context.Player.Hitbox.Y < Hitbox.Y)
                    Jump();
                
            }

            base.Tick(deltaTime);
        }
    }
}