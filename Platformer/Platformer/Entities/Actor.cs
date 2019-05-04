using System;
using System.Linq;
using Platformer.Game;

namespace Platformer.Entities
{
    public enum Axis
    {
        Horizontal, Vertical
    }

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
                        Context.Enemies.Remove(monster);
                    _health = value;
                }
                else
                    _health = value;
            }
        }
        
        /// <summary>
        /// Направления перемещения
        /// </summary>
        public enum Direction
        {
            Right,
            Left
        };

        /// <summary>
        /// Верхняя граница скорости перемещения по горизонтали
        /// </summary>
        protected double maxHorizontalVelocity = 20 * 100;

        /// <summary>
        /// Верхняя граница скорости перемещения по вертикали
        /// </summary>
        protected double maxVerticalVelocity = 20 * 100;

        /// <summary>
        /// Скорость перемещения
        /// (по умолчанию — нулевая)
        /// </summary>
        protected Vector velocity = Vector.Zero();

        /// <inheritdoc />
        /// <summary>
        /// Создаёт экземпляр актора по контексту и занимаемой области
        /// </summary>
        /// <param name="context">Контекст (мир, где находится актор)</param>
        /// <param name="hitbox">Занимаемая область</param>
        public Actor(World context, HitBox hitbox) : base(context, hitbox)
        {

        }

        /// <inheritdoc />
        /// <summary>
        /// Конструктор, создающий экземпляр актора по его размеру
        /// (нужен для того, чтобы можно было заготорвить актора до его непосредственного размещения в том или ином мире)
        /// </summary>
        /// <param name="size">Размер актора</param>
        public Actor(Vector size) : base(size)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Конструктор актора по умолчанию
        /// </summary>
        public Actor()
        {
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
        protected double runningSpeed = 200;

        /// <summary>
        /// Сила, с которой актор отталкивается от земли при прыжке
        /// </summary>
        protected double jumpHeight = 200 * 9.8;

        /// <summary>
        /// Побуждает актора бежать в указанном направлении
        /// </summary>
        /// <param name="direction"></param>
        public void Run(Direction direction)
        {
            Pull(direction == Direction.Right
                ? new Vector {x = runningSpeed, y = 0,}
                : new Vector {x = -runningSpeed, y = 0,});
        }

        /// <summary>
        /// Ограничивает скорость перемещения заданными константами
        /// </summary>
        private void CutVelocity()
        {
            if (velocity.x > maxHorizontalVelocity)
                velocity.x = maxHorizontalVelocity;
            if (velocity.x < -maxHorizontalVelocity)
                velocity.x = -maxHorizontalVelocity;

            if (velocity.y > maxVerticalVelocity)
                velocity.y = maxVerticalVelocity;
            if (velocity.y < -maxVerticalVelocity)
                velocity.y = -maxVerticalVelocity;
        }

        /// <summary>
        /// Проверяет, возможно ли переместиться в указанном направлении
        /// </summary>
        /// <param name="plannedVelocity"></param>
        /// <returns></returns>
        private bool MovementIsPossble(Vector plannedVelocity)
        {
            var tempHitbox = new HitBox(Hitbox.X + plannedVelocity.x, Hitbox.Y + plannedVelocity.y, Hitbox.Width, Hitbox.Height);

            return Context.SolidEntities.All(e => e == this || !e.Intersects(tempHitbox));
        }

        private void MoveTillIntersect(double distance, Axis axis, HitBox target)
        {
            switch (axis)
            {
                case Axis.Horizontal:
                    Move(new Vector
                        { x = target.X - Hitbox.X + (distance > 0 ? -Hitbox.Width - 0.1 : target.Width + 0.1), y = 0 });
                    return;

                case Axis.Vertical:
                    Move(new Vector
                        { x = 0, y = target.Y - Hitbox.Y + (distance >= 0 ? -Hitbox.Height - 0.1 : target.Height + 0.1) });
                    return;

                default:
                    throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
            }
        }

        private void MoveInAxis(double distance, Axis axis)
        {
            if (Math.Abs(distance) < 1e-10) return;
            var tempHitbox = new HitBox(Hitbox,  axis, distance);

            foreach (var e in Context.SolidEntities)
                if (e != this && e.Intersects(tempHitbox))
                {
                    MoveTillIntersect(distance, axis, e.Hitbox);
                    if (e is Actor actor)
                    {
                        var d = distance > 0 ? -500 : 500;
                        if (axis == Axis.Horizontal)
                            d *= 500;
                        else if (d < 0)
                        {
                            actor.Health -= 10;
                        }

                        velocity.SetAxis(axis, d);
                    }
                    else
                        velocity.SetAxis(axis, 0);
                    return;
                }

            Move(Vector.InAxis(distance, axis));
        }

        private void MoveHorizontal(double x)
        {
            MoveInAxis(x, Axis.Horizontal);
        }

        private void MoveVertical(double y)
        {
            MoveInAxis(y, Axis.Vertical);
        }

        /// <summary>
        /// Проверяет, стоит ли актор на замле
        /// </summary>
        /// <returns></returns>
        private bool FreeFromDown()
            => MovementIsPossble(new Vector { x = 0, y = 3});

        /// <summary>
        /// Побуждает актора к прыжку
        /// </summary>
        public void Jump()
        {
            if (!FreeFromDown())
                velocity += new Vector { x = 0, y = -jumpHeight };
        }

        /// <summary>
        /// Если актор стоит на земле — прекращает его горизонтальное движение
        /// </summary>
        public void TryToStop()
        {
            if (!FreeFromDown())
                velocity.x = 0;
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
            CutVelocity();

            var direction = velocity * deltaTime;

            MoveHorizontal(direction.x);
            MoveVertical(direction.y);

            velocity += Context.Gravity * deltaTime;
            velocity.x *= Math.Pow(1e-4, deltaTime);
        }
    }
}
