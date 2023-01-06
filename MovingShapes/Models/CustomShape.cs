using MovingShapes.Events;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace MovingShapes.Models
{
    [Serializable()]
    public abstract class CustomShape
    {
        public bool IsStoped { get; set; }
        public object Name { get; set; }
        public int CurrentNumber { get; set; }
        protected int _moveStepX = 3; //?
        protected int _moveStepY = 3;
        public int MoveStepX { get; set; }
        public int MoveStepY { get; set; }
        protected bool _isDeserialized;
        public Point Position { get; set; }

        public CustomShape()
        {
            Name = new();
        }

        abstract public void Move(ref Point maxPoint);
        abstract public void Draw();
        abstract public void AddToCanvas(Canvas canvas);
        public event EventHandler<ShapesIntersectionEventArgs>? ShapesIntersection;
        protected void Intersected(object sender, ShapesIntersectionEventArgs e)
        {
            OnShapesIntersected(sender, e);
        }
        protected void OnShapesIntersected(object? sender, ShapesIntersectionEventArgs e)
        {
            var temp = Volatile.Read(ref ShapesIntersection);
            if (temp != null)
            {
                temp(sender, e);
            }
        }
        //abstract public void ShapesIntersected(object sender, ShapesIntersectionEventArgs e);
    }
}
