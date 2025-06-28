using EventWebApp.Application.DTOs;

namespace EventWebApp.Application.Interfaces
{
  public interface IAuthValidationService
  {
    Task<(bool isValid, object? result)> ValidateLoginAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<(bool isValid, object? result)> ValidateRegisterDetailsAsync(RegisterDetailsRequest request, CancellationToken cancellationToken);
    Task<(bool isValid, object? result)> ValidateRefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken);
  }
}