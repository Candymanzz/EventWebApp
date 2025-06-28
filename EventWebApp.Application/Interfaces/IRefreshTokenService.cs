using EventWebApp.Application.DTOs;

namespace EventWebApp.Application.Interfaces
{
  public interface IRefreshTokenService
  {
    Task<AuthResponse> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<AuthResponse> GenerateTokensForUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task InvalidateRefreshTokenAsync(Guid userId, CancellationToken cancellationToken = default);
  }
}