using Platformer.Game;
using Platformer.Physics;
using static System.Math;

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
                    actor.Health -= 300;
                    if (actor.Health <= 0)
                    {
                        IRectangle rectangle = Hitbox;
                        Context.RemoveBody(_body);
                        double x = rectangle.Width;
                        double y = rectangle.Height;
                        double vol = actor.Hitbox.Width * actor.Hitbox.Height + x * y;

                        _body = Context.CreateMonsterBody(new HitBox(rectangle.X, rectangle.Y, Sqrt(vol * x / y), Sqrt(vol * y / x)));
                        _body.Tag = this;
                        _body.AddCollisionEvent((o, d) => { if (d == Direction.Down) { canJump = true; jumpEntity = o as Entity; } });
                    }
                }
            }
            canJump = false;
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