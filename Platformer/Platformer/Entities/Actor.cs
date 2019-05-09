using System;
using System.Linq;
using Platformer.Game;
using Platformer.Physics;

namespace Platformer.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// "Актёр", "Действующее лицо", живая сущность, населяющая миры игры
    /// </summary>
    internal class Actor : Entity
    {
        public double MaxHealth { get; protected set; }


        private double _health;

        public double Health {
            get => _health;
            internal set
            {
                if (value > MaxHealth)
                    _health = MaxHealth;
                else
                    _health = value;
            }
        }
        
        /// <summary>
        /// Верхняя граница скорости перемещения по горизонтали
        /// </summary>
        protected double maxHorizontalVelocity = 200;

        /// <summary>
        /// Верхняя граница скорости перемещения по вертикали
        /// </summary>
        protected double maxVerticalVelocity = 20 * 100;

        /// <summary>
        /// Скорость перемещения
        /// (по умолчанию — нулевая)
        /// </summary>
        protected Vector velocity = Vector.Zero();

        protected bool canJump = false;
        protected Entity jumpEntity;

        /// <inheritdoc />
        /// <summary>
        /// Создаёт экземпляр актора по контексту и занимаемой области
        /// </summary>
        /// <param name="context">Контекст (мир, где находится актор)</param>
        /// <param name="hitbox">Занимаемая область</param>
        public Actor(World context, IBody body) : base(context, body)
        {
            _body.AddCollisionEvent((o, d) => { if (d == Direction.Down) { canJump = true; jumpEntity = o as Entity; } });
        }

        /// <summary>
        /// Прибавляет к скорости актора указанное значение. "Тянет" актора по вектору.
        /// </summary>
        /// <param name="force"></param>
        public void Pull(Vector force)
        {
            velocity += force;
        }

        /// <summary>
        /// Скорость, с которой актор двигается
        /// </summary>
        protected double runningSpeed = 10;

        /// <summary>
        /// Сила, с которой актор отталкивается от земли при прыжке
        /// </summary>
        protected double jumpHeight = 300;

        /// <summary>
        /// Побуждает актора бежать в указанном направлении
        /// </summary>
        /// <param name="direction"></param>
        public void RunRight()
        {
            _body.Accelerate(new Vector {x = runningSpeed, y = 0});
        }

        public void RunLeft()
        {
            _body.Accelerate(new Vector { x = -runningSpeed, y = 0});
        }

        /// <summary>
        /// Побуждает актора к прыжку
        /// </summary>
        public virtual void Jump()
        {
            if (canJump)
            {
                _body.Accelerate(new Vector { x = 0, y = -jumpHeight });
                jumpEntity._body.Accelerate(new Vector { x = 0, y = jumpHeight });
                if (jumpEntity is Actor actor)
                    actor.Health -= 30;
            }
            canJump = false;
        }

        /// <summary>
        /// Если актор стоит на земле — прекращает его горизонтальное движение
        /// </summary>
        public void TryToStop()
        {
            Console.WriteLine("InvalidProgramException");
        }

        public override void Tick(double deltaTime)
        {
            base.Tick(deltaTime);
            canJump = false;
        }
    }
}
