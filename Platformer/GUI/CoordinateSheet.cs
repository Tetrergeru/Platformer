using System;
using System.Drawing;
using Platformer;

namespace GUI
{
    internal class CoordinateSheet
    {
        private const double HorizontalAdjustPersent = 0.1;

        private const double VerticalAdjustPersent = 0.1;

        public Vector coordinates = Vector.Zero();

        public CoordinateSheet(double w, double h)
        {
            Width = w;
            Height = h;
            Scale = 100;
        }

        public double Width { get; set; }

        public double Height { get; set; }

        public double Scale { get; private set; }

        public Rectangle Transform(IRectangle hitbox)
        {
            return new Rectangle
            {
                X = (int) Math.Floor((hitbox.X - coordinates.x) * Scale),
                Y = (int) Math.Floor((hitbox.Y - coordinates.y) * Scale),
                Width = (int) Math.Ceiling(hitbox.Width * Scale),
                Height = (int) Math.Ceiling(hitbox.Height * Scale)
            };
        }

        public void ChangeScale(double newScale, IRectangle playerLocation)
        {
            Console.WriteLine($"Scale = {newScale}");
            var playerCoords = coordinates * -1 + playerLocation.Coordinates;
            coordinates = coordinates + playerCoords * (1 - Scale / newScale);
            Scale = newScale;
        }

        public void SetSize(double w, double h)
        {
            Width = w;
            Height = h;
        }
        
        public void AdjustBy(IRectangle hitbox)
        {
            var playerCoords = (coordinates * -1 + hitbox.Coordinates) * Scale;

            if (playerCoords.x < Width * HorizontalAdjustPersent)
                coordinates.x = hitbox.X - Width * HorizontalAdjustPersent / Scale;
            else if (playerCoords.x > Width * (1 - HorizontalAdjustPersent))
                coordinates.x = hitbox.X - Width * (1 - HorizontalAdjustPersent) / Scale;

            if (playerCoords.y < Height * VerticalAdjustPersent)
                coordinates.y = hitbox.Y - Height * VerticalAdjustPersent / Scale;
            else if (playerCoords.y > Height * (1 - VerticalAdjustPersent))
                coordinates.y = hitbox.Y - Height * (1 - VerticalAdjustPersent) / Scale;
        }

        public void Move(Vector delta)
        {
            coordinates += delta * (1 / Scale);
        }
    }
}