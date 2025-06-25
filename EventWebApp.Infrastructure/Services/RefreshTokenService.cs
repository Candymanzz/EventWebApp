using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;
using EventWebApp.Core.Interfaces;
using EventWebApp.Core.Model;
using Microsoft.Extensions.Configuration;

namespace EventWebApp.Infrastructure.Services
{
  public class RefreshTokenService : IRefreshTokenService
  {
    private readonly ITokenService tokenService;
    private readonly IUserRepository userRepository;
    private readonly IConfiguration configuration;

    public RefreshTokenService(
        ITokenService tokenService,
        IUserRepository userRepository,
        IConfiguration configuration)
    {
      this.tokenService = tokenService;
      this.userRepository = userRepository;
      this.configuration = configuration;
    }

    public async Task<AuthResponse> RefreshAccessTokenAsync(string refreshToken)
    {
      var user = await userRepository.GetByRefreshTokenAsync(refreshToken);
      if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
      {
        throw new UnauthorizedAccessException("Invalid or expired refresh token");
      }

      // Генерируем только новый access токен, refresh токен остается тем же
      var newAccessToken = tokenService.GenerateAccessToken(user);

      return new AuthResponse
      {
        AccessToken = newAccessToken,
        RefreshToken = refreshToken
      };
    }

    public async Task<AuthResponse> GenerateTokensForUserAsync(Guid userId)
    {
      var user = await userRepository.GetByIdAsync(userId);
      if (user == null)
      {
        throw new ArgumentException("User not found");
      }

      var accessToken = tokenService.GenerateAccessToken(user);
      var refreshToken = tokenService.GenerateRefreshToken();
      var refreshTokenDays = configuration.GetValue<int>("JwtSettings:RefreshTokenDays", 7);
      var expiry = DateTime.UtcNow.AddDays(refreshTokenDays);

      await userRepository.UpdateRefreshTokenAsync(userId, refreshToken, expiry);

      return new AuthResponse
      {
        AccessToken = accessToken,
        RefreshToken = refreshToken
      };
    }

    public async Task<bool> ValidateRefreshTokenAsync(string refreshToken)
    {
      var user = await userRepository.GetByRefreshTokenAsync(refreshToken);
      return user != null && user.RefreshTokenExpiryTime > DateTime.UtcNow;
    }

    public async Task InvalidateRefreshTokenAsync(Guid userId)
    {
      await userRepository.UpdateRefreshTokenAsync(userId, null, null);
    }
  }
}