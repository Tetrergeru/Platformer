using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Platformer
{
    public enum State
    {
        Running,
        Pause,
    }

    /// <summary>
    /// Форма, на кторой происходит отрисовка игрового мира
    /// </summary>
    class Window : Form
    {
        private State gameState = State.Running;
        
        private PauseWindow pause;

        private CoordinateSheet coordSheet;

        public void ChangeScale(double delta)
        {
            coordSheet.ChangeScale(coordSheet.Scale * delta, game.Player.Hitbox);
        }

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

            pause = new PauseWindow();

            pict = new Bitmap(Width, Height);
            drawer = Graphics.FromImage(pict);
            drawer.InterpolationMode = InterpolationMode.NearestNeighbor;
            drawer.PixelOffsetMode = PixelOffsetMode.Half;

            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
            SizeChanged += OnSizeChanged;
        }

        public void Pause()
        {
            game.Stop();
            gameState = State.Pause;
            pause.Show();
        }

        public void Continue()
        {
            gameState = State.Running;
            game.Start();
        }

        /// <summary>
        /// Обрабатывает нажатие клавиши на клавиатуре
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (gameState == State.Running)
            {
                Console.WriteLine("***");
                if (Platformer.Controls.Control.StopTime == Platformer.Controls.ControlFromKey(e.KeyCode))
                {
                    Pause();
                }
                else
                    game.KeysPressed.Add(Platformer.Controls.ControlFromKey(e.KeyCode));
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (gameState == State.Running)
                game.KeysPressed.Remove(Platformer.Controls.ControlFromKey(e.KeyCode));
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            pict = new Bitmap(Width, Height);
            drawer = Graphics.FromImage(pict);
            drawer.InterpolationMode = InterpolationMode.NearestNeighbor;
            drawer.PixelOffsetMode = PixelOffsetMode.Half;
            coordSheet.SetSize(Width, Height);
            coordSheet.ChangeScale(Width / game.Player.Hitbox.Width / 50, game.Player.Hitbox);
        }

        /// <summary>
        /// Очищает картинку
        /// </summary>
        public void Clear(Color color)
        {
            drawer.Clear(color);
        }

        /// <summary>
        /// Рисует на картинке данное изображение в данном прямоугольнике
        /// </summary>
        /// <param name="image"></param>
        /// <param name="hitbox"></param>
        public void Draw(Bitmap image, HitBox hitbox)
        {
            if (gameState == State.Running)
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

        public void Draw(Entity entity)
        {
            Draw(entity.Texture.Image, entity.Hitbox);
        }

        public void Draw(IEnumerable<Entity> entities)
        {
            foreach (var e in entities)
            {
                Draw(e);
            }
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
