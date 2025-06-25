using EventWebApp.Application.DTOs;

namespace EventWebApp.Application.Interfaces
{
  public interface IRefreshTokenService
  {
    Task<AuthResponse> RefreshAccessTokenAsync(string refreshToken);
    Task<AuthResponse> GenerateTokensForUserAsync(Guid userId);
    Task<bool> ValidateRefreshTokenAsync(string refreshToken);
    Task InvalidateRefreshTokenAsync(Guid userId);
  }
}