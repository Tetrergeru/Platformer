using System.Drawing;
using System.Windows.Forms;

namespace Platformer
{
    /// <inheritdoc />
    /// <summary>
    /// Класс кнопки, который используется в этом проекте
    /// </summary>
    public class Button : PictureBox
    {
        /// <summary>
        /// Выводит текст на кнопку
        /// </summary>
        /// <param name="value">Текст, который будет выведен</param>
        public void SetText(string value)
        {
            var g = Graphics.FromImage(Image);
            g.Clear(Color.Gray);

            g.DrawRectangle(new Pen(Color.Black,5), new Rectangle(0, 0, Width, Height));
            var stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var font = new Font("Times New Roman", 15);

            g.DrawString(value, font, Brushes.Black, new Rectangle(0,0,Width, Height), stringFormat);
            Image = Image;
        }

        /// <inheritdoc />
        /// <summary>
        /// Создаёт кнопку данной ширины и высоты
        /// </summary>
        /// <param name="width">Ширина кнопки</param>
        /// <param name="height">Высота кнопки</param>
        public Button(int width, int height)
        {
            Width = width;
            Height = height;
            Image = new Bitmap(width, height);
        }
    }
}