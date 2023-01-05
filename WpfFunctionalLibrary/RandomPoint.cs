using System;
using System.Windows;

namespace WpfFunctionalLibrary
{
    public class RandomPoint
    {
        private static Random _randomValue = new();

        public static Point GetRadomPoint(int canvasWidth, int canvasHeight)
        {
            _randomValue = new Random();
            return new Point(_randomValue.Next(0, canvasWidth), _randomValue.Next(0, canvasHeight));
        }
    }
}