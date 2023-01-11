using MovingShapes.Events;
using MovingShapes.Exceptions;
using MovingShapes.Exceptions.CustomException;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfFunctionalLibrary;

namespace MovingShapes.Models
{
    [Serializable()]
    public class CustomSquare : CustomShape
    {
        private const int _side = 40;
        [NonSerialized]
        private Rectangle _rectangle;
        [NonSerialized]
        private SolidColorBrush _colorBrush = new((Color)ColorConverter.ConvertFromString("#FB8500"));
        private Dictionary<CustomShape, bool> _intersectedSquares = new();

        public CustomSquare()
        {
            _isDeserialized = true;
            _rectangle = new Rectangle() { Width = _side, Height = _side, Fill = _colorBrush, StrokeThickness = 1, Stroke = Brushes.DarkSlateBlue };
        }
        public CustomSquare(Canvas canvas)
        {
            Name = "Square";
            MoveStepX = _moveStepX;
            MoveStepY = _moveStepY;
            Position = RandomPoint.GetRadomPoint((int)canvas.ActualWidth - 2 * _side, (int)canvas.ActualHeight - 2 * _side);
            _rectangle = new Rectangle() { Width = _side, Height = _side, Fill = _colorBrush, StrokeThickness = 1, Stroke = Brushes.DarkSlateBlue };
            canvas.Children.Add(_rectangle);

            Draw();
        }
        public override void Draw()
        {
            Canvas.SetLeft(_rectangle, Position.X);
            Canvas.SetTop(_rectangle, Position.Y);
        }

        public override void Move(ref Point maxPoint)
        {
            CheckForShapeIsOutOfWindow(ref maxPoint);
            if (_isDeserialized)
            {
                _moveStepX = MoveStepX;
                _moveStepY = MoveStepY;
                _isDeserialized = false;
            }

            if (Position.X + _side + _moveStepX > maxPoint.X || Position.X + _moveStepX < 0)
            {
                _moveStepX *= -1;
            }
            if (Position.Y + _side + _moveStepY > maxPoint.Y || Position.Y + _moveStepY < 0)
            {
                _moveStepY *= -1;
            }

            MoveStepX = _moveStepX;
            MoveStepY = _moveStepY;

            Position = new Point(Position.X + _moveStepX, Position.Y);
            Position = new Point(Position.X, Position.Y + _moveStepY);

            Draw();
        }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            if (_rectangle is null)
            {
                _colorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB8500"));
                _rectangle = new Rectangle() { Width = _side, Height = _side, Fill = _colorBrush, StrokeThickness = 1, Stroke = Brushes.DarkSlateBlue };
            }
        }
        public override void AddToCanvas(Canvas canvas)
        {
            canvas.Children.Add(_rectangle);
            Draw();
        }

        public override async Task CheckForIntersection(List<CustomShape> shapes)
        {
            if (!IsEventShapesIntersectionNull())
            {
                _rectangle.Fill = Brushes.Blue;
                foreach (CustomShape shape in shapes)
                {
                    if (shape.GetType() != typeof(CustomSquare) || shape.GetHashCode() == GetHashCode())
                    {
                        continue;
                    }
                    var vector = new Vector(shape.Position.X - Position.X, shape.Position.Y - Position.Y);
                    bool isAlreadyIntersected = true;
                    if (vector.Length <= _side)
                    {
                        try
                        {
                            isAlreadyIntersected = _intersectedSquares[shape];
                        }
                        catch (KeyNotFoundException)
                        {
                            _intersectedSquares.Add(shape, false);
                            isAlreadyIntersected = _intersectedSquares[shape];
                        }

                        if (!isAlreadyIntersected)
                        {
                            _intersectedSquares[shape] = true;
                            var intersectionPoint = new Point((shape.Position.X + Position.X) / 2.0, (shape.Position.Y + Position.Y) / 2.0);
                            await Task.Run(() => { Intersected(_rectangle, new ShapesIntersectionEventArgs(ref intersectionPoint)); });
                        }
                    }
                    else if (isAlreadyIntersected && vector.Length > _side)
                    {
                        _intersectedSquares[shape] = false;
                    }
                }
            }
            else
            {
                _rectangle.Fill = _colorBrush;
            }
        }

        protected override void CheckForShapeIsOutOfWindow(ref Point maxPoint)
        {
            if (Position.X + _side > maxPoint.X || Position.Y + _side > maxPoint.Y)
            {
                throw new CustomException<ShapeIsOutOfWindowExceptionArgs>(new ShapeIsOutOfWindowExceptionArgs(this));
            }
        }
        public override void ReturnShapeToWindow(ref Point maxPoint)
        {
            if (Position.Y + _side < maxPoint.Y && Position.X + _side > maxPoint.X)
            {
                Position = new Point(maxPoint.X - _side, Position.Y);
            }
            else if (Position.X + _side < maxPoint.X && Position.Y + _side > maxPoint.Y)
            {
                Position = new Point(Position.X, maxPoint.Y - _side);
            }
            else if (Position.X + _side > maxPoint.X && Position.Y + _side > maxPoint.Y)
            {
                Position = new Point(maxPoint.X - _side, maxPoint.Y - _side);
            }
            Draw();
        }
    }
}
