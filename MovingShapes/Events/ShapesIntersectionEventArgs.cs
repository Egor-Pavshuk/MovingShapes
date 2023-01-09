using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
