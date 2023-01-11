using System;

namespace MovingShapes.Exceptions.CustomException
{
    [Serializable]
    public abstract class ExceptionArgs
    {
        public virtual string? Message { get => string.Empty; }
    }
}
