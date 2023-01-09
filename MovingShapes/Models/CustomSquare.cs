using MovingShapes.Events;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private SolidColorBrush _colorBrush;
        private Dictionary<CustomShape, bool> _intersectedSquares = new Dictionary<CustomShape, bool>();

        public CustomSquare()
        {
            _isDeserialized = true;
            _colorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB703"));
            _rectangle = new Rectangle() { Width = _side, Height = _side, Fill = _colorBrush, StrokeThickness = 1, Stroke = Brushes.DarkSlateBlue };
        }
        public CustomSquare(Canvas canvas)
        {
            Name = "Square";
            MoveStepX = _moveStepX;
            MoveStepY = _moveStepY;
            _colorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB703"));
            Position = RandomPoint.GetRadomPoint((int)canvas.ActualWidth - 2 * _side, (int)canvas.ActualHeight - 2 * _side);
            _rectangle = new Rectangle() { Width = _side, Height = _side, Fill =_colorBrush, StrokeThickness = 1, Stroke = Brushes.DarkSlateBlue };
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
                _rectangle = new Rectangle() { Width = _side, Height = _side, Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB703")), StrokeThickness = 1, Stroke = Brushes.DarkSlateBlue };
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

        //public override void ShapesIntersected(object sender, ShapesIntersectionEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
