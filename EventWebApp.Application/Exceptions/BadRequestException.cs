namespace EventWebApp.Application.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException()
            : base("Bad request") { }

        public BadRequestException(string message)
            : base(message) { }

        public BadRequestException(string message, Exception innerException)
            : base(message, innerException) { }

        public BadRequestException(string message, string errorCode)
            : base(message, errorCode) { }

        public BadRequestException(string message, string errorCode, Exception innerException)
            : base(message, errorCode, innerException) { }
    }
}
