namespace EventWebApp.Application.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException()
            : base("Entity not found") { }

        public NotFoundException(string message)
            : base(message) { }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        public NotFoundException(string message, string errorCode)
            : base(message, errorCode) { }

        public NotFoundException(string message, string errorCode, Exception innerException)
            : base(message, errorCode, innerException) { }
    }
}
