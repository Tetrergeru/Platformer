using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Platformer
{
    /// <summary>
    /// Форма, на кторой происходит отрисовка игрового мира
    /// </summary>
    class Window : Form
    {
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

            pict = new Bitmap(Width, Height);
            drawer = Graphics.FromImage(pict);
            
            KeyDown += OnKeyDown;
        }

        /// <summary>
        /// Обрабатывает нажатие клавиши на клавиатуре
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D:
                    {
                        game.Player.Run(Actor.Direction.Right);
                        break;
                    }
                case Keys.A:
                    {
                        game.Player.Run(Actor.Direction.Left);
                        break;
                    }
                case Keys.W:
                    {
                        game.Player.Jump();
                        break;
                    }
                case Keys.S:
                    {
                        game.Player.TryToStop();
                        break;
                    }
                case Keys.Z:
                    {
                        game.Stop();
                        break;
                    }
                case Keys.X:
                    {
                        game.Start();
                        break;
                    }
                case Keys.P:
                    {
                        game.Player.Hitbox.MoveTo(new Vector { x = 40, y = 40});
                        break;
                    }
            }
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
        public void Draw(Bitmap image, Rectangle rect)
        {
            drawer.DrawImage(image, rect);
        }

        /// <summary>
        /// рисует на кртинке прямоугольник данного цвета
        /// </summary>
        /// <param name="color"></param>
        /// <param name="rect"></param>
        public void Draw(Color color, Rectangle rect)
        {
            drawer.DrawRectangle(new Pen(color), rect);
        }

        /// <summary>
        /// Переносит содержимое картинки на экран
        /// </summary>
        public void Flush()
        {
            var screen = CreateGraphics();
            screen.DrawImage(pict, 0, 0);
        }
    }
}
