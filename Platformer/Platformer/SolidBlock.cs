using System;
using System.Drawing;
using System.Windows.Forms;

namespace Platformer
{
    internal class SolidBlock : Entity
    {
        /// <summary>
        /// Конструктор, создающий экземпляр блока по его миру и расположению
        /// </summary>
        /// <param name="context"></param>
        /// <param name="hitbox"></param>
        public SolidBlock(World context, HitBox hitbox) : base(context, hitbox) { }

        /// <summary>
        /// Конструктор, создающий экземпляр блока по его размеру
        /// (нужен для того, чтобы можно было заготорвить сущность до его непосредственного размещения в том или ином мире)
        /// </summary>
        /// <param name="size"></param>
        public SolidBlock(Vector size) : base(size) { }
        
        /// <summary>
        /// Конструктор блока по умолчанию
        /// </summary>
        public SolidBlock() { }

        public static SolidBlock Make(World context, HitBox hitbox, Bitmap texture, FillType ft, double scale = 1)
        {
            var result = new SolidBlock(context, hitbox);
            result.Texture.AddTexture(texture, ft, scale);
            return result;
        }
    }
}