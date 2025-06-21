namespace EventWebApp.Application.Exceptions
{
    public class AlreadyExistsException : BaseException
    {
        public AlreadyExistsException()
            : base("Entity already exists") { }

        public AlreadyExistsException(string message)
            : base(message) { }

        public AlreadyExistsException(string message, Exception innerException)
            : base(message, innerException) { }

        public AlreadyExistsException(string message, string errorCode)
            : base(message, errorCode) { }

        public AlreadyExistsException(string message, string errorCode, Exception innerException)
            : base(message, errorCode, innerException) { }
    }
}
