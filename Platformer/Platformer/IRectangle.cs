using System.Dynamic;

namespace Platformer
{
    public interface IRectangle
    {
        double X { get; }

        double Y { get; }

        double X2 { get; }

        double Y2 { get; }

        double Width { get; }

        double Height { get; }

        Vector Coordinates { get; }
    }
}