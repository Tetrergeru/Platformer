using Platformer.Game;
using Platformer.Physics;

namespace Platformer.Entities
{
    /// <summary>
    /// Класс, представляющий сущность игрового мира
    /// </summary>
    internal class Entity
    {
        public IBody _body;
        /// <summary>
        /// Приоритет отрисовки
        /// </summary>
        public double DrawPriority { get; set; }

        /// <summary>
        /// Область занимаемая сущностью
        /// </summary>
        public IRectangle Hitbox => _body.Rectangle;
        
        /// <summary>
        /// Мир, в котором эта сущность расположена
        /// </summary>
        public World Context { get; set; }

        public string Texture { get; set; }

        /// <summary>
        /// Конструктор, создающий экземпляр сущности по её миру и расположению
        /// </summary>
        /// <param name="context"></param>
        /// <param name="hitbox"></param>
        public Entity(World context, IBody body)
        {
            Context = context;
            _body = body;
            _body.Tag = this;
            Texture = "";//new Texture((int)Math.Round(Hitbox.Width), (int)Math.Round(Hitbox.Height));
        }

        /// <summary>
        /// Перемещает сущность по направлению вектора
        /// </summary>
        /// <param name="velocity">Вектор скорости</param>
        public void Move(Vector velocity)
            => _body.Move(velocity);

        public void MoveTo(Vector location)
            => _body.MoveTo(location);

        public void RemoveBody()
        {
            Context.RemoveBody(_body);
        }

        public virtual void Tick(double deltaTime)
        {
            //Texture.Tick(deltaTime);
        }
    }
}
