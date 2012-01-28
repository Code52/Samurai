using System;

namespace SamuraiServer.Data.Providers
{
    // TODO: there's got to be an algorithm for calculating distance using tiles

    public class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }

        public double DistanceFrom(Point other)
        {
            var deltaX = Math.Abs(other.X - X);
            var deltaY = Math.Abs(other.Y - Y);

            return Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));

        }
    }
}