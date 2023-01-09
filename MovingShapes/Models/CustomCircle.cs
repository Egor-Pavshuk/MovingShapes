using MovingShapes.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfFunctionalLibrary;

namespace MovingShapes.Models
{
    [Serializable()]
    public class CustomCircle : CustomShape
    {
        private const int _radius = 40;
        [NonSerialized]
        private Ellipse _circle;
        private Dictionary<CustomShape, bool> _intersectedCircles = new Dictionary<CustomShape, bool>();

        public CustomCircle()
        {
            _isDeserialized = true;
            _circle = new Ellipse() { Width = _radius, Height = _radius, Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB8500")), StrokeThickness = 1, Stroke = Brushes.DarkSlateBlue };
        }
        public CustomCircle(Canvas canvas)
        {
            Name = "Circle";
            MoveStepX = _moveStepX;
            MoveStepY = _moveStepY;
            Position = RandomPoint.GetRadomPoint((int)canvas.ActualWidth - 2 * _radius, (int)canvas.ActualHeight - 2 * _radius);

            _circle = new Ellipse() { Width = _radius, Height = _radius, Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB8500")), StrokeThickness = 1, Stroke = Brushes.DarkSlateBlue };

            canvas.Children.Add(_circle);

            Draw();
        }
        public override void Draw()
        {
            Canvas.SetLeft(_circle, Position.X);
            Canvas.SetTop(_circle, Position.Y);
        }

        public override void Move(ref Point maxPoint)
        {
            if (_isDeserialized)
            {
                _moveStepX = MoveStepX;
                _moveStepY = MoveStepY;
                _isDeserialized = false;
            }

            if (Position.X + _radius + _moveStepX > maxPoint.X || Position.X + _moveStepX < 0)
            {
                _moveStepX *= -1;
            }
            if (Position.Y + _radius + _moveStepY > maxPoint.Y || Position.Y + _moveStepY < 0)
            {
                _moveStepY *= -1;
            }

            MoveStepX = _moveStepX;
            MoveStepY = _moveStepY;

            Position = new Point(Position.X + _moveStepX, Position.Y);
            Position = new Point(Position.X, Position.Y + _moveStepY);
            Draw();
        }
        public override async Task CheckForIntersection(List<CustomShape> shapes)
        {
            if (!IsEventShapesIntersectionNull())
            {
                _circle.Fill = Brushes.Blue;
                foreach (CustomShape shape in shapes)
                {
                    if (shape.GetType() != typeof(CustomCircle) || shape.GetHashCode() == GetHashCode())
                    {
                        continue;
                    }
                    var vector = new Vector(shape.Position.X - Position.X, shape.Position.Y - Position.Y);
                    bool isAlreadyIntersected = true;

                    if (vector.Length <= _radius)
                    {                        
                        try
                        {
                            isAlreadyIntersected = _intersectedCircles[shape];
                        }
                        catch (KeyNotFoundException)
                        {
                            _intersectedCircles.Add(shape, false);
                            isAlreadyIntersected = _intersectedCircles[shape];
                        }

                        if (!isAlreadyIntersected)
                        {
                            _intersectedCircles[shape] = true;
                            var intersectionPoint = new Point((shape.Position.X + Position.X) / 2.0, (shape.Position.Y + Position.Y) / 2.0);
                            await Task.Run(() => { Intersected(_circle, new ShapesIntersectionEventArgs(ref intersectionPoint)); });
                        }
                    }
                    else if (isAlreadyIntersected && vector.Length > _radius)
                    {
                        _intersectedCircles[shape] = false;
                    }
                }
            }
        }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            _circle ??= new Ellipse() { Width = _radius, Height = _radius, Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB8500")), StrokeThickness = 1, Stroke = Brushes.DarkSlateBlue };
        }
        public override void AddToCanvas(Canvas canvas)
        {
            canvas.Children.Add(_circle);
            Draw();
        }

        //public override void ShapesIntersected(object sender, ShapesIntersectionEventArgs e)
        //{
        //    _circle.Fill = Brushes.Black;
        //    //throw new NotImplementedException();
        //}
    }
}
