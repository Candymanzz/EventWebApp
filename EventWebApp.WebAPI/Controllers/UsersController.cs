using System.Security.Claims;
using EventWebApp.Application.DTOs;
using EventWebApp.Application.UseCases.User;
using EventWebApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventWebApp.WebAPI.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class UsersController : ControllerBase
  {
    private readonly RegisterUserUseCase registerUserUseCase;
    private readonly GetUserByIdUseCase getUserByIdUseCase;
    private readonly GetUsersByEventUseCase getUserByEventUseCase;
    private readonly RegisterUserToEventWithValidationUseCase registerUserToEventWithValidationUseCase;
    private readonly CancelUserFromEventWithValidationUseCase cancelUserFromEventWithValidationUseCase;
    private readonly GetUserEventsWithValidationUseCase getUserEventsWithValidationUseCase;

    public UsersController(
        RegisterUserUseCase registerUserUseCase,
        GetUserByIdUseCase getUserByIdUseCase,
        GetUsersByEventUseCase getUserByEventUseCase,
        RegisterUserToEventWithValidationUseCase registerUserToEventWithValidationUseCase,
        CancelUserFromEventWithValidationUseCase cancelUserFromEventWithValidationUseCase,
        GetUserEventsWithValidationUseCase getUserEventsWithValidationUseCase
    )
    {
      this.registerUserUseCase = registerUserUseCase;
      this.getUserByIdUseCase = getUserByIdUseCase;
      this.getUserByEventUseCase = getUserByEventUseCase;
      this.registerUserToEventWithValidationUseCase = registerUserToEventWithValidationUseCase;
      this.cancelUserFromEventWithValidationUseCase = cancelUserFromEventWithValidationUseCase;
      this.getUserEventsWithValidationUseCase = getUserEventsWithValidationUseCase;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
      var result = await getUserByIdUseCase.ExecuteAsync(id, cancellationToken);
      return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Register(
        [FromBody] UserRegistrationRequest userRegistrationRequest,
        CancellationToken cancellationToken
    )
    {
      var result = await registerUserUseCase.ExecuteAsync(userRegistrationRequest, cancellationToken);
      return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet("by-event/{eventId}")]
    public async Task<IActionResult> GetUsersByEvent(Guid eventId, CancellationToken cancellationToken)
    {
      var result = await getUserByEventUseCase.ExecuteAsync(eventId, cancellationToken);
      return Ok(result);
    }

    [Authorize(Policy = "UserOnly")]
    [HttpPost("register-to-event")]
    public async Task<IActionResult> RegisterToEvent(
        [FromBody] RegisterUserToEventRequest request,
        CancellationToken cancellationToken
    )
    {
      var result = await registerUserToEventWithValidationUseCase.ExecuteAsync(User, request.EventId, cancellationToken);
      var (statusCode, data) = result.ToHttpResponse();

      return statusCode switch
      {
        200 => Ok(data),
        400 => BadRequest(data),
        _ => StatusCode(statusCode, data)
      };
    }

    [Authorize(Policy = "UserOnly")]
    [HttpPost("cancel-from-event")]
    public async Task<IActionResult> CancelFromEvent(
        [FromBody] RegisterUserToEventRequest request,
        CancellationToken cancellationToken
    )
    {
      var result = await cancelUserFromEventWithValidationUseCase.ExecuteAsync(User, request.EventId, cancellationToken);
      var (statusCode, data) = result.ToHttpResponse();

      return statusCode switch
      {
        200 => Ok(data),
        400 => BadRequest(data),
        _ => StatusCode(statusCode, data)
      };
    }

    [Authorize(Policy = "UserOnly")]
    [HttpGet("me/events")]
    public async Task<IActionResult> GetMyEvents(CancellationToken cancellationToken)
    {
      var result = await getUserEventsWithValidationUseCase.ExecuteAsync(User, cancellationToken);
      var (statusCode, data) = result.ToHttpResponse();

      return statusCode switch
      {
        200 => Ok(data),
        400 => BadRequest(data),
        _ => StatusCode(statusCode, data)
      };
    }
  }
}
