using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Platformer
{
    class Window : Form
    {
        private Bitmap pict;

        private Graphics drawer;

        private Game game;

        public Window(Game game)
        {
            this.game = game;

            Width = 1000;
            Height = 500;

            pict = new Bitmap(Width, Height);
            drawer = Graphics.FromImage(pict);
            
            KeyDown += OnKeyDown;
        }

        const double MovingSpeed = 7;

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D)
            {
                game.Player.Pull(new Vector { x = MovingSpeed, y = 0 });
                Console.WriteLine("D");
            }
            if (e.KeyCode == Keys.A)
            {
                game.Player.Pull(new Vector { x = -MovingSpeed, y = 0 });
            }
            if (e.KeyCode == Keys.W)
            {
                game.Player.Jump();
            }
        }
        
        public void Clear()
        {
            drawer.Clear(Color.White);
        }

        public void Draw(Bitmap image, Rectangle rect)
        {
            drawer.DrawImage(image, rect);
        }

        public void Draw(Color color, Rectangle rect)
        {
            drawer.DrawRectangle(new Pen(color), rect);
        }

        public void Flush()
        {
            var screen = CreateGraphics();
            screen.DrawImage(pict, 0, 0);
        }
    }
}
