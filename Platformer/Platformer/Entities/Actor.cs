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
        /// Верхняя граница скорости перемещения по горизонтали
        /// </summary>
        private const double MaxHorizontalVelocity = 20 * 100;

        /// <summary>
        /// Верхняя граница скорости перемещения по вертикали
        /// </summary>
        private const double MaxVerticalVelocity = 20 * 100;

        /// <summary>
        /// Скорость перемещения
        /// (по умолчанию — нулевая)
        /// </summary>
        public Vector velocity = Vector.Zero();

        /// <summary>
        /// Создаёт экземпляр актора по контексту и занимаемой области
        /// </summary>
        /// <param name="context">Контекст (мир, где находится актор)</param>
        /// <param name="hitbox">Занимаемая область</param>
        public Actor(World context, HitBox hitbox) : base(context, hitbox)
        {

        }

        /// <summary>
        /// Конструктор, создающий экземпляр актора по его размеру
        /// (нужен для того, чтобы можно было заготорвить актора до его непосредственного размещения в том или ином мире)
        /// </summary>
        /// <param name="size">Размер актора</param>
        public Actor(Vector size) : base(size)
        {
        }

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
        protected double RunningSpeed = 200;

        /// <summary>
        /// Сила, с которой актор отталкивается от земли при прыжке
        /// </summary>
        protected double JumpHeight = 200 * 9.8;

        /// <summary>
        /// Побуждает актора бежать в указанном направлении
        /// </summary>
        /// <param name="direction"></param>
        public void Run(Direction direction)
        {
            if (direction == Direction.Right)
                Pull(new Vector {x = RunningSpeed, y = 0,});
            else
                Pull(new Vector {x = -RunningSpeed, y = 0,});
        }

        /// <summary>
        /// Ограничивает скорость перемещения заданными константами
        /// </summary>
        private void CutVelocity()
        {
            if (velocity.x > MaxHorizontalVelocity)
                velocity.x = MaxHorizontalVelocity;
            if (velocity.x < -MaxHorizontalVelocity)
                velocity.x = -MaxHorizontalVelocity;

            if (velocity.y > MaxVerticalVelocity)
                velocity.y = MaxVerticalVelocity;
            if (velocity.y < -MaxVerticalVelocity)
                velocity.y = -MaxVerticalVelocity;
        }

        /// <summary>
        /// Проверяет, возможно ли переместиться в указанном направлении
        /// </summary>
        /// <param name="velocity"></param>
        /// <returns></returns>
        private bool MovementIsPossble(Vector velocity)
        {
            var tempHitbox = new HitBox(Hitbox.X + velocity.x, Hitbox.Y + velocity.y, Hitbox.Width, Hitbox.Height);

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
                    if (e is Actor)
                    {
                        var d = distance > 0 ? -500 : 500;
                        if (axis == Axis.Horizontal)
                            d *= 1000;

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
            if (Flight || !FreeFromDown())
                velocity += new Vector { x = 0, y = -JumpHeight };
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
