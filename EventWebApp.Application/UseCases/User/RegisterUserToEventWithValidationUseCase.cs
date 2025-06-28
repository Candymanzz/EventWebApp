using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;
using EventWebApp.Application.UseCases.User;
using System.Security.Claims;

namespace EventWebApp.Application.UseCases.User
{
  public class RegisterUserToEventWithValidationUseCase
  {
    private readonly RegisterUserToEventUseCase _registerUserToEventUseCase;
    private readonly IUserContextService _userContextService;

    public RegisterUserToEventWithValidationUseCase(
      RegisterUserToEventUseCase registerUserToEventUseCase,
      IUserContextService userContextService)
    {
      _registerUserToEventUseCase = registerUserToEventUseCase;
      _userContextService = userContextService;
    }

    public async Task<UserActionResult> ExecuteAsync(ClaimsPrincipal user, Guid eventId, CancellationToken cancellationToken)
    {
      var userId = _userContextService.GetUserId(user);
      if (userId == null)
      {
        return UserActionResult.Failure("User not authenticated");
      }

      try
      {
        await _registerUserToEventUseCase.ExecuteAsync(userId.Value, eventId, cancellationToken);
        return UserActionResult.Success();
      }
      catch (Exception ex)
      {
        return UserActionResult.Failure(ex.Message);
      }
    }
  }
}