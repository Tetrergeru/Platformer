using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GUI.Textures
{
    internal class Texture : global::GUI.Textures.ITexture
    {
        public int Width => Image.Width;

        public int Height => Image.Height;
        
        public Bitmap Image { get; set; }

        public Texture(int w, int h)
        {
            Image = new Bitmap(w, h);
        }

        public void AddTexture(Bitmap picture, global::GUI.Textures.FillType ft, double scale = 1)
        {
            DrawTexture(Image, picture, ft, scale);
        }

        public void Tick(double deltaTime) { }

        public static void DrawTexture(Bitmap image, Bitmap picture, global::GUI.Textures.FillType ft, double scale)
        {
            var graphics = Graphics.FromImage(image);
            graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            graphics.PixelOffsetMode = PixelOffsetMode.Half;

            switch (ft)
            {
                case global::GUI.Textures.FillType.Repeat:
                {
                    for (int i = 0; i <= image.Width / (picture.Width * scale); i++)
                    for (int j = 0; j <= image.Height / (picture.Height * scale); j++)
                        graphics.DrawImage(picture,
                            (int) (i * picture.Width * scale),
                            (int) (j * picture.Height * scale),
                            (int) (picture.Width * scale),
                            (int) (picture.Height * scale));
                    break;
                }
                case global::GUI.Textures.FillType.Stretch:
                {
                    graphics.DrawImage(picture, 0, 0, image.Width, image.Height);
                    break;
                }
                case global::GUI.Textures.FillType.StretchDown:
                {
                    var newWidth = (int)Math.Ceiling(picture.Width * (image.Height / (double)picture.Height));
                    for (int i = 0; i <= image.Width / newWidth; i++)
                        graphics.DrawImage(picture, i * newWidth, 0, newWidth + 1, image.Height + 1);
                    break;
                }
            }
        }
    }
}
