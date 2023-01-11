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
    public class CustomTriangle : CustomShape
    {
        private Point _p1;
        private Point _p2;
        private Point _p3;
        private Dictionary<CustomShape, bool> _intersectedTriangles = new Dictionary<CustomShape, bool>();

        [NonSerialized]
        private Line line1;
        [NonSerialized]
        private Line line2;
        [NonSerialized]
        private Line line3;

        public CustomTriangle()
        {
            _isDeserialized = true;
            _p1.X = 60;
            _p1.Y = 0;
            _p2.X = 5;
            _p2.Y = 60;
            _p3.X = 100;
            _p3.Y = 60;

            line1 = new Line() { X1 = _p1.X, X2 = _p2.X, Y1 = _p1.Y, Y2 = _p2.Y, Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#023047")) };
            line2 = new Line() { X1 = _p1.X, X2 = _p3.X, Y1 = _p1.Y, Y2 = _p3.Y, Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#023047")) };
            line3 = new Line() { X1 = _p3.X, X2 = _p2.X, Y1 = _p3.Y, Y2 = _p2.Y, Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#023047")) };
        }
        public CustomTriangle(Canvas canvas)
        {
            Name = "Triangle";
            MoveStepX = _moveStepX;
            MoveStepY = _moveStepY;

            _p1.X = 60;
            _p1.Y = 0;
            _p2.X = 5;
            _p2.Y = 60;
            _p3.X = 100;
            _p3.Y = 60;

            line1 = new Line() { X1 = _p1.X, X2 = _p2.X, Y1 = _p1.Y, Y2 = _p2.Y, Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#023047")) };
            line2 = new Line() { X1 = _p1.X, X2 = _p3.X, Y1 = _p1.Y, Y2 = _p3.Y, Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#023047")) };
            line3 = new Line() { X1 = _p3.X, X2 = _p2.X, Y1 = _p3.Y, Y2 = _p2.Y, Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#023047")) };

            Position = RandomPoint.GetRadomPoint((int)canvas.ActualWidth - 2 * (int)GetOuterRadius(), (int)canvas.ActualHeight - 2 * (int)GetOuterRadius());

            _p1 = Vector.Add(new Vector(Position.X, Position.Y), _p1);
            _p2 = Vector.Add(new Vector(Position.X, Position.Y), _p2);
            _p3 = Vector.Add(new Vector(Position.X, Position.Y), _p3);

            AddToCanvas(canvas);
        }

        public override void Draw()
        {
            Canvas.SetLeft(line1, Position.X);
            Canvas.SetTop(line1, Position.Y);
            Canvas.SetLeft(line2, Position.X);
            Canvas.SetTop(line2, Position.Y);
            Canvas.SetLeft(line3, Position.X);
            Canvas.SetTop(line3, Position.Y);
        }

        public override void Move(ref Point maxPoint)
        {
            CheckForShapeIsOutOfWindow(ref maxPoint);
            if (_isDeserialized)
            {
                _p1 = Vector.Add(new Vector(Position.X, Position.Y), new Point(60, 0));
                _p2 = Vector.Add(new Vector(Position.X, Position.Y), new Point(5, 60));
                _p3 = Vector.Add(new Vector(Position.X, Position.Y), new Point(100, 60));

                _moveStepX = MoveStepX;
                _moveStepY = MoveStepY;

                _isDeserialized = false;
            }


            if (_p1.X + _moveStepX > maxPoint.X || _p2.X + _moveStepX > maxPoint.X || _p3.X + _moveStepX > maxPoint.X)
            {
                _moveStepX *= -1;
            }
            else if (_p1.X + _moveStepX < 0 || _p2.X + _moveStepX < 0 || _p3.X + _moveStepX < 0)
            {
                _moveStepX *= -1;
            }

            if (_p1.Y + _moveStepY > maxPoint.Y || _p2.Y + _moveStepY > maxPoint.Y || _p3.Y + _moveStepY > maxPoint.Y)
            {
                _moveStepY *= -1;
            }
            else if (_p1.Y + _moveStepY < 0 || _p2.Y + _moveStepY < 0 || _p3.Y + _moveStepY < 0)
            {
                _moveStepY *= -1;
            }

            _p1.X += _moveStepX;
            _p2.X += _moveStepX;
            _p3.X += _moveStepX;
            _p1.Y += _moveStepY;
            _p2.Y += _moveStepY;
            _p3.Y += _moveStepY;

            MoveStepX = _moveStepX;
            MoveStepY = _moveStepY;

            Position = new Point(Position.X + _moveStepX, Position.Y);
            Position = new Point(Position.X, Position.Y + _moveStepY);

            Draw();
        }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            if (line1 is null || line2 is null || line3 is null)
            {
                _p1.X = 60;
                _p1.Y = 0;
                _p2.X = 5;
                _p2.Y = 60;
                _p3.X = 100;
                _p3.Y = 60;

                line1 = new Line() { X1 = _p1.X, X2 = _p2.X, Y1 = _p1.Y, Y2 = _p2.Y, Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#023047")) };
                line2 = new Line() { X1 = _p1.X, X2 = _p3.X, Y1 = _p1.Y, Y2 = _p3.Y, Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#023047")) };
                line3 = new Line() { X1 = _p3.X, X2 = _p2.X, Y1 = _p3.Y, Y2 = _p2.Y, Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#023047")) };

                _p1 = Vector.Add(new Vector(Position.X, Position.Y), new Point(60, 0));
                _p2 = Vector.Add(new Vector(Position.X, Position.Y), new Point(5, 60));
                _p3 = Vector.Add(new Vector(Position.X, Position.Y), new Point(100, 60));
            }
        }

        public override void AddToCanvas(Canvas canvas)
        {
            canvas.Children.Add(line1);
            canvas.Children.Add(line2);
            canvas.Children.Add(line3);
            Draw();
        }

        public override async Task CheckForIntersection(List<CustomShape> shapes)
        {
            if (!IsEventShapesIntersectionNull())
            {
                foreach (CustomShape shape in shapes)
                {
                    if (shape.GetType() != typeof(CustomTriangle) || shape.GetHashCode() == GetHashCode())
                    {
                        continue;
                    }
                    var vector = new Vector(shape.Position.X - Position.X, shape.Position.Y - Position.Y);
                    bool isAlreadyIntersected = true;
                    if (vector.Length <= 2 * GetOuterRadius())
                    {
                        try
                        {
                            isAlreadyIntersected = _intersectedTriangles[shape];
                        }
                        catch (KeyNotFoundException)
                        {
                            _intersectedTriangles.Add(shape, false);
                            isAlreadyIntersected = _intersectedTriangles[shape];
                        }

                        if (!isAlreadyIntersected)
                        {
                            _intersectedTriangles[shape] = true;
                            var intersectionPoint = new Point((shape.Position.X + Position.X) / 2.0, (shape.Position.Y + Position.Y) / 2.0);
                            await Task.Run(() => { Intersected(null, new ShapesIntersectionEventArgs(ref intersectionPoint)); });
                        }
                    }
                    else if (isAlreadyIntersected && vector.Length > 2 * GetOuterRadius())
                    {
                        _intersectedTriangles[shape] = false;
                    }
                }
            }
        }
        public override void ReturnShapeToWindow(ref Point maxPoint)
        {
            double radius = GetOuterRadius();
            if (Position.Y + radius < maxPoint.Y && Position.X + 2 * radius > maxPoint.X)
            {
                Position = new Point(maxPoint.X - 2.5 * radius, Position.Y);
            }
            else if (Position.X + 2 * radius < maxPoint.X && Position.Y + radius > maxPoint.Y)
            {
                Position = new Point(Position.X, maxPoint.Y - 1.5 * radius);
            }
            else if (Position.X + 2 * radius > maxPoint.X && Position.Y + radius > maxPoint.Y)
            {
                Position = new Point(maxPoint.X - 2.5 * radius, maxPoint.Y - 1.5 * radius);
            }
            Vector translateVector = new(Position.X, Position.Y);
            _p1 = Vector.Add(translateVector, new Point(line1.X1, line1.Y1));
            _p2 = Vector.Add(translateVector, new Point(line1.X2, line1.Y2));
            _p3 = Vector.Add(translateVector, new Point(line2.X2, line1.Y2));
            Draw();
        }

        private double GetOuterRadius()
        {
            double radius;
            var vector1 = new Vector(line1.X1 - line1.X2, line1.Y1 - line1.Y2);
            var vector2 = new Vector(line2.X1 - line2.X2, line2.Y1 - line2.Y2);
            var vector3 = new Vector(line3.X1 - line3.X2, line3.Y1 - line3.Y2);

            double semiPerimeter = (vector1.Length + vector2.Length + vector3.Length) / 2;
            double area = Math.Sqrt(semiPerimeter * (semiPerimeter - vector1.Length) * (semiPerimeter - vector2.Length) * (semiPerimeter - vector3.Length));
            radius = 1 / 4.0 * vector1.Length * vector2.Length * vector3.Length / area;
            return radius;
        }

        protected override void CheckForShapeIsOutOfWindow(ref Point maxPoint)
        {
            double radius = GetOuterRadius();
            if (Position.X + 2 * radius > maxPoint.X || Position.Y + radius > maxPoint.Y)
            {
                throw new CustomException<ShapeIsOutOfWindowExceptionArgs>(new ShapeIsOutOfWindowExceptionArgs(this));
            }
        }
    }
}
