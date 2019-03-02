using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Platformer
{
    public class Button : PictureBox
    {
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

            var font = new Font("Times New Roman", 20);

            g.DrawString(value, font, Brushes.Black, new Rectangle(0,0,Width, Height), stringFormat);
            Image = Image;
        }

        public Button(int width, int height)
        {
            BackColor = Color.Transparent;
            Width = width;
            Height = height;
            Image = new Bitmap(width, height);
        }
    }
}