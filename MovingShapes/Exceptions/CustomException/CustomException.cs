using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace MovingShapes.Exceptions.CustomException
{
    public sealed class CustomException<TExceptionArgs> : Exception, ISerializable
        where TExceptionArgs : ExceptionArgs
    {
        private const string _args = "Args";
        private readonly TExceptionArgs? _exceptionArgs;
        public TExceptionArgs? Args { get { return _exceptionArgs; } }
        public CustomException(string? message = null, Exception? innerException = null) : this(null, message, innerException) { }
        public CustomException(TExceptionArgs? args, string? message = null, Exception? innerException = null) : base(message, innerException)
        {
            _exceptionArgs = args;
        }
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        private CustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _exceptionArgs = (TExceptionArgs?)info.GetValue(_args, typeof(TExceptionArgs));
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(_args, _exceptionArgs);
            base.GetObjectData(info, context);
        }
        public override string Message
        {
            get
            {
                string baseMsg = base.Message;
                return (_exceptionArgs == null) ? baseMsg : baseMsg + " (" + _exceptionArgs.Message + ")";
            }
        }
        public override bool Equals(object? obj)
        {
            CustomException<TExceptionArgs>? other = obj as CustomException<TExceptionArgs>;
            if (other == null)
                return false;
            return object.Equals(_exceptionArgs, other._exceptionArgs) && base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
