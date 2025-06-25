using System.Net;
using System.Text.Json;
using EventWebApp.Application.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EventWebApp.WebAPI.Middleware
{
  public class ExceptionHandlingMiddleware
  {
    private readonly RequestDelegate requestDelegate;
    private readonly ILogger<ExceptionHandlingMiddleware> logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate requestDelegate,
        ILogger<ExceptionHandlingMiddleware> logger
    )
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
          errors = ex.Errors.Select(e => new
          {
            field = e.PropertyName,
            error = e.ErrorMessage,
          }),
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
      }
      catch (NotFoundException ex)
      {
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        context.Response.ContentType = "application/json";

        var errorResponse = new { message = ex.Message, errorCode = ex.ErrorCode };

        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
      }
      catch (BadRequestException ex)
      {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = "application/json";

        var errorResponse = new { message = ex.Message, errorCode = ex.ErrorCode };

        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
      }
      catch (AlreadyExistsException ex)
      {
        context.Response.StatusCode = (int)HttpStatusCode.Conflict;
        context.Response.ContentType = "application/json";

        var errorResponse = new { message = ex.Message, errorCode = ex.ErrorCode };

        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
      }
      catch (ConflictException ex)
      {
        context.Response.StatusCode = (int)HttpStatusCode.Conflict;
        context.Response.ContentType = "application/json";

        var errorResponse = new { message = ex.Message, errorCode = ex.ErrorCode };

        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
      }
      catch (UnauthorizedException ex)
      {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        context.Response.ContentType = "application/json";

        var errorResponse = new { message = ex.Message, errorCode = ex.ErrorCode };

        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
      }
      catch (ForbiddenException ex)
      {
        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        context.Response.ContentType = "application/json";

        var errorResponse = new { message = ex.Message, errorCode = ex.ErrorCode };

        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
      }
      catch (UnauthorizedAccessException ex)
      {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(new { message = ex.Message })
        );
      }
      catch (DbUpdateException ex)
      {
        logger.LogError(ex, "Database update exception");

        if (ex.InnerException?.Message?.Contains("duplicate key") == true)
        {
          context.Response.StatusCode = (int)HttpStatusCode.Conflict;
          context.Response.ContentType = "application/json";

          var errorResponse = new
          {
            message = "User is already registered for this event",
            errorCode = ErrorCodes.UserAlreadyRegisteredForEvent
          };

          await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
        else
        {
          context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
          context.Response.ContentType = "application/json";

          await context.Response.WriteAsync(
              JsonSerializer.Serialize(new { message = "An unexpected error occurred" })
          );
        }
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "Unhandled exception");

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(new { message = "An unexpected error occurred" })
        );
      }
    }
  }
}
