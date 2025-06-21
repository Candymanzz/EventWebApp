namespace EventWebApp.Application.Exceptions
{
    public abstract class BaseException : Exception
    {
        public string ErrorCode { get; }

        protected BaseException(string message)
            : base(message) { }

        protected BaseException(string message, Exception innerException)
            : base(message, innerException) { }

        protected BaseException(string message, string errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        protected BaseException(string message, string errorCode, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
