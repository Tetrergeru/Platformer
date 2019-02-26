using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Platformer
{
    class Drawer
    {
        public enum FillType { Repeat, Stretch, StretchDown };

        public Bitmap Image { get; set; }

        public Drawer(int w, int h)
        {
            Image = new Bitmap(w, h);
        }

        public void AddTexture(Bitmap image, FillType ft)
        {
            var graphics = Graphics.FromImage(Image);
            graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            switch (ft)
            {
                case FillType.Repeat:
                    {
                        for (int i = 0; i <= Image.Width / image.Width; i++)
                            for (int j = 0; j <= Image.Height / image.Height; j++)
                                graphics.DrawImageUnscaled(image, i * image.Width, j * image.Height);
                        break;
                    }
                case FillType.Stretch:
                    {
                        graphics.DrawImage(image, 0, 0, Image.Width, Image.Height);
                        break;
                    }
                case FillType.StretchDown:
                    {
                        var newWidth = (int)Math.Round(image.Width * (Image.Height / (double)image.Height));
                        for (int i = 0; i <= Image.Width / newWidth; i++)
                            graphics.DrawImage(image, i * newWidth, 0, newWidth, Image.Height);
                        break;
                    }
            }
        }
    }
}
