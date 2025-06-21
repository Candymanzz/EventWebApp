namespace EventWebApp.Application.Exceptions
{
    public static class ErrorCodes
    {
        // User related errors
        public const string UserNotFound = "USER_NOT_FOUND";
        public const string UserAlreadyExists = "USER_ALREADY_EXISTS";
        public const string UserNotRegisteredForEvent = "USER_NOT_REGISTERED_FOR_EVENT";
        public const string UserAlreadyRegisteredForEvent = "USER_ALREADY_REGISTERED_FOR_EVENT";

        // Event related errors
        public const string EventNotFound = "EVENT_NOT_FOUND";
        public const string EventAlreadyExists = "EVENT_ALREADY_EXISTS";
        public const string EventFull = "EVENT_FULL";
        public const string EventDateInPast = "EVENT_DATE_IN_PAST";

        // Authentication errors
        public const string Unauthorized = "UNAUTHORIZED";
        public const string Forbidden = "FORBIDDEN";
        public const string InvalidCredentials = "INVALID_CREDENTIALS";
        public const string TokenExpired = "TOKEN_EXPIRED";
        public const string TokenInvalid = "TOKEN_INVALID";

        // Validation errors
        public const string ValidationFailed = "VALIDATION_FAILED";
        public const string InvalidRequest = "INVALID_REQUEST";

        // Configuration errors
        public const string ConfigurationError = "CONFIGURATION_ERROR";
        public const string JwtSecretNotConfigured = "JWT_SECRET_NOT_CONFIGURED";

        // General errors
        public const string InternalServerError = "INTERNAL_SERVER_ERROR";
        public const string Conflict = "CONFLICT";
        public const string NotFound = "NOT_FOUND";
        public const string BadRequest = "BAD_REQUEST";
    }
}
