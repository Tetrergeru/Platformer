using System;
using System.Collections.Generic;
using System.Drawing;

namespace Platformer.GUI
{
    public class TextureAnimated : ITexture
    {
        private double _timer;

        public double timerDelay, afterAnimationDelay;

        private List<Bitmap> Images { get; }

        public Bitmap Image
        {
            get
            {
                if (Images.Count * timerDelay > _timer)
                    return Images[(int)Math.Ceiling(_timer / timerDelay) % Images.Count];

                if (Images.Count * timerDelay + afterAnimationDelay < _timer)
                    _timer = 0;

                return Images[0];
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
            _timer += deltaTime;
        }

        public void AddTexture(Bitmap picture, FillType ft, double scale = 1)
        {
            var image = new Bitmap(Width, Height);
            Texture.DrawTexture(image, picture, ft, scale);
            Images.Add(image);
        }
    }
}