using FluentValidation;
using System.Net;
using System.Text.Json;

namespace EventWebApp.WebAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate requestDelegate;
        private readonly ILogger<ExceptionHandlingMiddleware> logger;

        public ExceptionHandlingMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionHandlingMiddleware> logger)
        {
            this.requestDelegate = requestDelegate;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await requestDelegate(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                var errorResponse = new
                {
                    message = "Validation failed",
                    errors = ex.Errors.Select(e => new { field = e.PropertyName, error = e.ErrorMessage })
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            }
            catch (UnauthorizedAccessException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    message = ex.Message
                }));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    message = "An unexpected error occurred"
                }));
            }
        }
    }
}
