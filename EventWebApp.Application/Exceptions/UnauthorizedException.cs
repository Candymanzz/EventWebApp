namespace EventWebApp.Application.Exceptions
{
    public class UnauthorizedException : BaseException
    {
        public UnauthorizedException()
            : base("Unauthorized access") { }

        public UnauthorizedException(string message)
            : base(message) { }

        public UnauthorizedException(string message, Exception innerException)
            : base(message, innerException) { }

        public UnauthorizedException(string message, string errorCode)
            : base(message, errorCode) { }

        public UnauthorizedException(string message, string errorCode, Exception innerException)
            : base(message, errorCode, innerException) { }
    }
}
