namespace EventWebApp.Application.Exceptions
{
    public class ConflictException : BaseException
    {
        public ConflictException()
            : base("Conflict occurred") { }

        public ConflictException(string message)
            : base(message) { }

        public ConflictException(string message, Exception innerException)
            : base(message, innerException) { }

        public ConflictException(string message, string errorCode)
            : base(message, errorCode) { }

        public ConflictException(string message, string errorCode, Exception innerException)
            : base(message, errorCode, innerException) { }
    }
}
