using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;
using EventWebApp.Application.UseCases.User;
using System.Security.Claims;

namespace EventWebApp.Application.UseCases.User
{
  public class GetUserEventsWithValidationUseCase
  {
    private readonly GetUserEventsUseCase _getUserEventsUseCase;
    private readonly IUserContextService _userContextService;

    public GetUserEventsWithValidationUseCase(
        GetUserEventsUseCase getUserEventsUseCase,
        IUserContextService userContextService)
    {
      _getUserEventsUseCase = getUserEventsUseCase;
      _userContextService = userContextService;
    }

    public async Task<UserEventsResult> ExecuteAsync(ClaimsPrincipal user, CancellationToken cancellationToken)
    {
      var userId = _userContextService.GetUserId(user);
      if (userId == null)
      {
        return UserEventsResult.Failure("User not authenticated");
      }

      try
      {
        var events = await _getUserEventsUseCase.ExecuteAsync(userId.Value, cancellationToken);
        return UserEventsResult.Success(events);
      }
      catch (Exception ex)
      {
        return UserEventsResult.Failure(ex.Message);
      }
    }
  }
}