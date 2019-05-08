using System.Drawing;

namespace GUI
{
    public enum FillType { Repeat, Stretch, StretchDown };

    public interface ITexture
    {
        int Width { get; }

        int Height { get; }

        Bitmap Image { get; }

        void AddTexture(Bitmap picture, FillType ft, double scale = 1);

        void Tick(double deltaTime);
    }
}