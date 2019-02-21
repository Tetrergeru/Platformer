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


        public Window()
        {
            Width = 1000;
            Height = 500;

            pict = new Bitmap(Width, Height);
            drawer = Graphics.FromImage(pict);
        }

        public void Clear()
        {
            drawer.Clear(Color.White);
        }

        public void Draw(Bitmap image, Rectangle rect)
        {
            drawer.DrawImage(image, rect);
        }

        public void Flush()
        {
            var screen = CreateGraphics();
            screen.DrawImage(pict, 0, 0);
        }
    }
}
