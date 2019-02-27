using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    /// <summary>
    /// "Актёр", "Действующее лицо", живая сущность, населяющая миры игры
    /// </summary>
    class Actor : Entity
    {
        public bool Flight;
        /// <summary>
        /// Направления перемещения
        /// </summary>
        public enum Direction { Right, Left };

        /// <summary>
        /// Верхняя граница скорости перемещения по горизонтали
        /// </summary>
        private const double MaxHorizontalVelocity = 20;

        /// <summary>
        /// Верхняя граница скорости перемещения по вертикали
        /// </summary>
        private const double MaxVerticalVelocity = 50;

        /// <summary>
        /// Ускорение (в нормальном случае — ускорение свободного падения)
        /// </summary>
        protected Vector acceleration = new Vector { x = 0, y = 1.2 };

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
        public Actor(Vector size) : base(size) { }

        /// <summary>
        /// Конструктор актора по умолчанию
        /// </summary>
        public Actor() : base() { }

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
        const double RunningSpeed = 1;

        /// <summary>
        /// Сила, с которой актор отталкивается от земли при прыжке
        /// </summary>
        private const double JumpHeight = 16;

        /// <summary>
        /// Побуждает актора бежать в указанном направлении
        /// </summary>
        /// <param name="direction"></param>
        public void Run(Direction direction)
        {
            if (direction == Direction.Right)
                Pull(new Vector { x = RunningSpeed, y = 0, });
            else
                Pull(new Vector { x = -RunningSpeed, y = 0, });
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

            foreach (var e in Context.Entities)
                if (e != this && e.Intersects(tempHitbox))
                {
                    return false;
                }
            return true;
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

            var direction = velocity * deltaTime * 100;

            if (MovementIsPossble(direction.ZeroY()))
                Move(direction.ZeroY());
            else
                velocity = velocity.ZeroX();

            if (MovementIsPossble(direction.ZeroX()))
                Move(direction.ZeroX());
            else
                velocity = velocity.ZeroY();

            velocity += acceleration;
            velocity.x *= Math.Pow(0.0005, deltaTime);
        }
    }
}
