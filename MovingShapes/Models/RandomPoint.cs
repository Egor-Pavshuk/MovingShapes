using System;
using System.Windows;

namespace MovingShapes.Models
{
    internal class CustomRandomPoint
    {
        private static Random _randomValue = new();

        public static Point GetRadomPoint(int canvasWidth, int canvasHeight)
        {
            _randomValue = new Random();
            return new Point(_randomValue.Next(0, canvasWidth), _randomValue.Next(0, canvasHeight));
        }
    }
}
