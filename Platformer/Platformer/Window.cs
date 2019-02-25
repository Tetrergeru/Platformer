using System;
using System.Windows;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Platformer
{
    /// <summary>
    /// Форма, на кторой происходит отрисовка игрового мира
    /// </summary>
    class Window : Form
    {

        private CoordinateSheet coordSheet;

        /// <summary>
        /// Картинка, куда мы будем рисовать все необходимые изображения, перед тем, как отобразить всё на экране
        /// </summary>
        private Bitmap pict;

        /// <summary>
        /// Поверхность для рисование на картинке
        /// </summary>
        private Graphics drawer;

        /// <summary>
        /// Игра, привязанная к окну
        /// </summary>
        private Game game;

        /// <summary>
        /// Создаёт экземпляр окна, по переданной игре
        /// </summary>
        /// <param name="game"></param>
        public Window(Game game)
        {
            this.game = game;

            Width = 1000;
            Height = 500;
            
            coordSheet = new CoordinateSheet(Width, Height);

            pict = new Bitmap(Width, Height);
            drawer = Graphics.FromImage(pict);
            drawer.InterpolationMode = InterpolationMode.NearestNeighbor;

            KeyDown += OnKeyDown;
        }
        
        /// <summary>
        /// Обрабатывает нажатие клавиши на клавиатуре
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            game.OnControlTrigger(Platformer.Controls.ControlFromKey(e.KeyCode));
        }
        
        /// <summary>
        /// Очищает картинку
        /// </summary>
        public void Clear()
        {
            drawer.Clear(Color.White);
        }

        /// <summary>
        /// Рисует на картинке данное изображение в данном прямоугольнике
        /// </summary>
        /// <param name="image"></param>
        /// <param name="rect"></param>
        public void Draw(Bitmap image, HitBox hitbox)
        {
            drawer.DrawImage(image, coordSheet.Transform(hitbox));
        }

        /// <summary>
        /// рисует на кртинке прямоугольник данного цвета
        /// </summary>
        /// <param name="color"></param>
        /// <param name="rect"></param>
        public void Draw(Color color, HitBox hitbox)
        {
            drawer.DrawRectangle(new Pen(color, 2), coordSheet.Transform(hitbox));
        }
        
        /// <summary>
        /// Переносит содержимое картинки на экран
        /// </summary>
        public void Flush()
        {
            var screen = CreateGraphics();
            screen.DrawImage(pict, 0, 0);
        }

        public void AdjustBy(HitBox hitbox)
            => coordSheet.AdjustBy(hitbox);
    }
}
