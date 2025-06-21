namespace EventWebApp.Application.Exceptions
{
    public class ForbiddenException : BaseException
    {
        public ForbiddenException()
            : base("Access forbidden") { }

        public ForbiddenException(string message)
            : base(message) { }

        public ForbiddenException(string message, Exception innerException)
            : base(message, innerException) { }

        public ForbiddenException(string message, string errorCode)
            : base(message, errorCode) { }

        public ForbiddenException(string message, string errorCode, Exception innerException)
            : base(message, errorCode, innerException) { }
    }
}
