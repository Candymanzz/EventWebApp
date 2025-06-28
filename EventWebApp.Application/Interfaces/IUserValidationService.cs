using EventWebApp.Application.DTOs;
using System.Security.Claims;

namespace EventWebApp.Application.Interfaces
{
  public interface IUserValidationService
  {
    Task<(bool isValid, Guid? userId)> ValidateUserAuthenticationAsync(ClaimsPrincipal user);
    Task<(bool isValid, object? result)> ValidateUserRegistrationAsync(UserRegistrationRequest request, CancellationToken cancellationToken);
  }
}