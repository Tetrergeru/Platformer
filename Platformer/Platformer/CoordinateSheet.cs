using System;
using System.Drawing;

namespace Platformer
{
    class CoordinateSheet
    {
        public double Width { get; set; }

        public double Height { get; set; }

        public double Scale { get; set; }

        public Vector Coordinates = Vector.Zero();

        private const double HorizontalAdjustPersent = 0.3;

        private const double VerticalalAdjustPersent = 0.3;

        public CoordinateSheet(double w, double h)
        {
            Width = w;
            Height = h;
            Scale = 1;
        }

        public Rectangle Transform(HitBox hitbox)
        {
            return new Rectangle
            {
                X = (int)Math.Round(hitbox.X - Coordinates.x),
                Y = (int)Math.Round(hitbox.Y - Coordinates.y),
                Width = (int)Math.Round(hitbox.Width),
                Height = (int)Math.Round(hitbox.Height),
            };
        }

        private void AdjustCoordinate(ref double coord, double playerCoord, double lesserBorder, double biggerBorder)
        {
            if (playerCoord < lesserBorder || playerCoord > biggerBorder)
                coord += playerCoord - (playerCoord < lesserBorder ? lesserBorder : biggerBorder);
        }

        private void CounstBordersAndAdjustCoordinate(ref double coord, double playerCoord, double dimension, double dimensionAdjustPersent)
        {
            var lesserBorder = dimension * dimensionAdjustPersent;
            var biggerBorder = dimension - lesserBorder;
            AdjustCoordinate(ref coord, playerCoord, lesserBorder, biggerBorder);
        }

        public void AdjustBy(HitBox hitbox)
        {
            var playerCoords = Coordinates * (-1) + new Vector { x = hitbox.X, y = hitbox.Y };
            CounstBordersAndAdjustCoordinate(ref Coordinates.x, playerCoords.x, Width, HorizontalAdjustPersent);
            CounstBordersAndAdjustCoordinate(ref Coordinates.y, playerCoords.y, Height, VerticalalAdjustPersent);
        }
    }
}
