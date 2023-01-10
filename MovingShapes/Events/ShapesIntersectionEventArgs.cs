using System;
using System.Windows;

namespace MovingShapes.Events
{
    public class ShapesIntersectionEventArgs : EventArgs
    {
        private readonly Point _pointOfIntersection;
        public ShapesIntersectionEventArgs(ref Point point)
        {
            _pointOfIntersection = point;
        }

        public Point PointOfIntersection { get => _pointOfIntersection; }

    }
}
