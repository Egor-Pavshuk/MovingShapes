using MovingShapes.Exceptions.CustomException;
using MovingShapes.Models;
using System;

namespace MovingShapes.Exceptions
{
    [Serializable]
    public sealed class ShapeIsOutOfWindowExceptionArgs : ExceptionArgs
    {
        private readonly CustomShape? _shape;
        public ShapeIsOutOfWindowExceptionArgs(CustomShape shape)
        {
            _shape = shape;
        }
        public CustomShape? Shape { get { return _shape; } }
        public override string? Message
        {
            get => (_shape == null) ? base.Message : "The shape " + _shape.Name + " on position: " + _shape.Position.ToString() + " is out of window.";
        }
    }
}
