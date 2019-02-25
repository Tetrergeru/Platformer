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
