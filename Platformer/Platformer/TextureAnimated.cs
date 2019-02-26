using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;

namespace Platformer
{
    public class TextureAnimated : ITexture
    {
        private double timer = 0;

        private int timerDelay;

        private List<Bitmap> Images;

        public Bitmap Image => Images.First(); 

        public int Width { get; }

        public int Height { get; }

        public TextureAnimated(int w, int h)
        {
            Images = new List<Bitmap>();
            Width = w;
            Height = h;
        }

        public void Tick(double deltaTime)
        {

        }

        public void AddTexture(Bitmap picture, FillType ft, double scale = 1)
        {
            var image = new Bitmap(Width, Height);
            Texture.DrawTexture(image, picture, ft, scale);
            Images.Add(image);
        }
    }
}