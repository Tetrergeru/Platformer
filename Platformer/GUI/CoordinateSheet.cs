using System;
using System.Drawing;
using Platformer;

namespace GUI
{
    class CoordinateSheet
    {
        public double Width { get; set; }

        public double Height { get; set; }

        public double Scale { get; private set; }

        public Vector coordinates = Vector.Zero();

        private const double HorizontalAdjustPersent = 0.1;

        private const double VerticalalAdjustPersent = 0.1;

        public CoordinateSheet(double w, double h)
        {
            Width = w;
            Height = h;
            Scale = 1;
        }

        public Rectangle Transform(IRectangle hitbox)
        {
            return new Rectangle
            {
                X = (int)Math.Floor((hitbox.X - coordinates.x) * Scale),
                Y = (int)Math.Floor((hitbox.Y - coordinates.y) * Scale),
                Width = (int)Math.Ceiling(hitbox.Width * Scale),
                Height = (int)Math.Ceiling(hitbox.Height * Scale),
            };
        }

        public void ChangeScale(double newScale, HitBox playerLocation)
        {
            var playerCoords = coordinates * (-1) + playerLocation.Coordinates;
            coordinates = coordinates + playerCoords * (newScale < Scale ? (1 - Scale / newScale) : Scale / newScale);
            Scale = newScale;
        }

        public void SetSize(double w, double h)
        {
            Width = w;
            Height = h;
        }

        private void AdjustCoordinate(ref double coord, double playerCoord, double playerSize, double lesserBorder, double biggerBorder)
        {
            if (playerCoord < lesserBorder || playerCoord + playerSize > biggerBorder)
                coord += playerCoord - (playerCoord < lesserBorder ? lesserBorder : (biggerBorder - playerSize));
        }

        private void CounstBordersAndAdjustCoordinate(ref double coord, double playerCoord, double playerSize, double dimension, double dimensionAdjustPersent)
        {
            var lesserBorder = dimension * dimensionAdjustPersent;
            var biggerBorder = dimension - lesserBorder;
            AdjustCoordinate(ref coord, playerCoord, playerSize, lesserBorder, biggerBorder);
        }

        public void AdjustBy(IRectangle hitbox)
        {
            var playerCoords = (coordinates * (-1) + hitbox.Coordinates) * Scale;
            CounstBordersAndAdjustCoordinate(ref coordinates.x, playerCoords.x, hitbox.Width * Scale, Width, HorizontalAdjustPersent);
            CounstBordersAndAdjustCoordinate(ref coordinates.y, playerCoords.y, hitbox.Height * Scale,Height, VerticalalAdjustPersent);
        }

        public void Move(Vector delta)
        {
            coordinates += delta * (1 / Scale);
        }
    }
}
