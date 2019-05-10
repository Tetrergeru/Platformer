using Platformer.Game;
using Platformer.Physics;
using static System.Math;

namespace Platformer.Entities
{
    internal class Monster : Actor
    {

        double defHealth = 100;
        IRectangle defRect;

        /// <inheritdoc />
        /// <summary>
        /// Конструктор, создающий экземпляр монстра по его миру и расположению
        /// </summary>
        /// <param name="context"></param>
        /// <param name="hitbox"></param>
        public Monster(World context, IBody body) : base(context, body)
        {
            MaxHealth = 50000;
            Health = defHealth;
            defRect = new HitBox(Hitbox);
            jumpHeight = 100;
        }

        public override void Jump()
        {
            if (canJump)
            {
                _body.Accelerate(new Vector { x = 0, y = -jumpHeight });

                if (jumpEntity == null)
                {
                    canJump = false;
                    return;
                }

                jumpEntity._body.Accelerate(new Vector { x = 0, y = jumpHeight });
                if (jumpEntity is Actor actor)
                {
                    double damage = 30;
                    double profit = Min(damage, actor.Health);
                    actor.Health -= damage;
                    Health += profit;
                    if (actor.Health <= 0)
                    {
                        IRectangle rectangle = Hitbox;
                        double k = Sqrt(1 + actor.Hitbox.Width * actor.Hitbox.Height / Hitbox.Width / Hitbox.Height);
                        _body.Resize(new Vector {x = k, y = k });
                    }
                }
            }
            canJump = false;
        }

        public override void Tick(double deltaTime)
        {
            double rectK = Hitbox.Width / defRect.Width;
            double HealthK = Health / defHealth;
            if (Abs(rectK - HealthK) > 0.0001)
            {
                double ratio = (HealthK / rectK - 1) / 10;
                _body.Resize(new Vector {x = ratio + 1, y = ratio  + 1});
            }

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