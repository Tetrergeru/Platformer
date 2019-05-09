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
            protected set
            {
                if (value > MaxHealth)
                    _health = MaxHealth;
                else if (value <= 0 && this is Monster monster)
                {
                    if (this is Monster)
                        Context.RemoveEntity(monster);
                    _health = value;
                }
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

        bool canJump = false;
        Entity jumpEntitie;

        /// <inheritdoc />
        /// <summary>
        /// Создаёт экземпляр актора по контексту и занимаемой области
        /// </summary>
        /// <param name="context">Контекст (мир, где находится актор)</param>
        /// <param name="hitbox">Занимаемая область</param>
        public Actor(World context, IBody body) : base(context, body)
        {
            _body.AddCollisionEvent((o, d) => {if(d == Direction.Down) canJump = true; jumpEntitie = o as Entity; });
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
        protected double runningSpeed = 200000000;

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
            _body.Pull(new Vector {x = runningSpeed, y = 0});
        }

        public void RunLeft()
        {
            _body.Pull(new Vector { x = -runningSpeed, y = 0});
        }

        /// <summary>
        /// Побуждает актора к прыжку
        /// </summary>
        public void Jump()
        {
            if (canJump)
            {
                _body.Accelerate(new Vector { x = 0, y = -jumpHeight });
                jumpEntitie._body.Accelerate(new Vector { x = 0, y = jumpHeight });
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
        }
    }
}
