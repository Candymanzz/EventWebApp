using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;
using System.Security.Claims;

namespace EventWebApp.WebAPI.Services
{
  public class UserValidationService : IUserValidationService
  {
    public Task<(bool isValid, Guid? userId)> ValidateUserAuthenticationAsync(ClaimsPrincipal user)
    {
      var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
      if (userId == null)
      {
        return Task.FromResult((false, (Guid?)null));
      }

      if (Guid.TryParse(userId, out var parsedUserId))
      {
        return Task.FromResult((true, (Guid?)parsedUserId));
      }

      return Task.FromResult((false, (Guid?)null));
    }

    public Task<(bool isValid, object? result)> ValidateUserRegistrationAsync(UserRegistrationRequest request, CancellationToken cancellationToken)
    {
      // Здесь можно добавить дополнительную валидацию если нужно
      return Task.FromResult((true, (object?)null));
    }
  }
}