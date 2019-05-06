using System;
using System.Linq;

namespace Platformer.Entities
{
    enum Axis
    {
        Horizontal, Vertical
    }

    /// <summary>
    /// "Актёр", "Действующее лицо", живая сущность, населяющая миры игры
    /// </summary>
    class Actor : Entity
    {
        public double MaxHealth { get; protected set; }
        private double health;
        public double Health {
            get => health;
            protected set
            {
                if (value > MaxHealth)
                    health = MaxHealth;
                else if (value <= 0)
                {
                    if (this is Monster monster)
                        Context.Enemies.Remove(monster);
                    else if (this is Player player)
                        player.GameOver();
                }
                else
                    health = value;
            }
        }

        public bool Flight;

        /// <summary>
        /// Направления перемещения
        /// </summary>
        public enum Direction
        {
            Right,
            Left
        };

        /// <summary>
        /// Создаёт экземпляр актора по контексту и занимаемой области
        /// </summary>
        /// <param name="context">Контекст (мир, где находится актор)</param>
        /// <param name="hitbox">Занимаемая область</param>
        public Actor(World context, HitBox hitbox) : base(context, hitbox)
        {
            movable = true;
            body.Movable = true;
        }

        /// <summary>
        /// Конструктор, создающий экземпляр актора по его размеру
        /// (нужен для того, чтобы можно было заготорвить актора до его непосредственного размещения в том или ином мире)
        /// </summary>
        /// <param name="size">Размер актора</param>
        public Actor(Vector size) : base(size)
        {
            movable = true;
        }

        /// <summary>
        /// Конструктор актора по умолчанию
        /// </summary>
        public Actor()
        {
            movable = true;
        }

        /// <summary>
        /// Прибавляет к скорости актора указанное значение. "Тянет" актора по вектору.
        /// </summary>
        /// <param name="force"></param>
        public void Pull(Vector force)
        {
            body.Pull(force);
        }

        /// <summary>
        /// Скорость, с которой актор двигается
        /// </summary>
        protected double RunningSpeed = 1000000;

        /// <summary>
        /// Сила, с которой актор отталкивается от земли при прыжке
        /// </summary>
        protected double JumpHeight = 30;

        /// <summary>
        /// Побуждает актора бежать в указанном направлении
        /// </summary>
        /// <param name="direction"></param>
        public void Run(Direction direction)
        {
            if (direction == Direction.Right)
                body.Pull(new Vector {x = RunningSpeed, y = 0,});
            else
                body.Pull(new Vector {x = -RunningSpeed, y = 0,});
        }

        /// <summary>
        /// Побуждает актора к прыжку
        /// </summary>
        public void Jump()
        {// TODO проверки в Jump
            body.Accelerate(new Vector { x = 0, y = -JumpHeight });
        }

        /// <summary>
        /// Если актор стоит на земле — прекращает его горизонтальное движение
        /// </summary>
        public void TryToStop()
        {// TODO tryToStop
            body.Movable = false;
        }

        public override void Tick(double deltaTime)
        {
            base.Tick(deltaTime);
            Move(deltaTime);
        }

        /// <summary>
        /// По возможности смещает актора с текущей свкоростью
        /// </summary>
        /// <param name="deltaTime">Время, прошедшее с предыдущего тика</param>
        public void Move(double deltaTime)
        {
            //TODO костыль, парный для tryToStop
            body.Movable = true;
        }
    }
}
