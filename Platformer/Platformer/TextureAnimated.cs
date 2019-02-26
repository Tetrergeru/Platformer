using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;

namespace Platformer
{
    public class TextureAnimated : ITexture
    {
        private double timer;

        public double timerDelay, afterAnimationDelay;

        private List<Bitmap> Images;

        public Bitmap Image
        {
            get
            {
                if (Images.Count * timerDelay > timer)
                    return Images[(int) Math.Ceiling(timer / timerDelay) % Images.Count];

                if (Images.Count * timerDelay + afterAnimationDelay < timer)
                    timer = 0;

                return Images.First();
            }
        }

        public int Width { get; }

        public int Height { get; }

        public TextureAnimated(int w, int h, double delay, double afterDelay)
        {
            Images = new List<Bitmap>();
            Width = w;
            Height = h;
            timerDelay = delay;
            afterAnimationDelay = afterDelay;
        }

        public void Tick(double deltaTime)
        {
            timer += deltaTime;
        }

        public void AddTexture(Bitmap picture, FillType ft, double scale = 1)
        {
            var image = new Bitmap(Width, Height);
            Texture.DrawTexture(image, picture, ft, scale);
            Images.Add(image);
        }
    }
}