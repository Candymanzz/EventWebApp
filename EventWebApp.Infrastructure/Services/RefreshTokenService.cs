using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;
using EventWebApp.Application.UseCases.User;
using EventWebApp.Core.Interfaces;
using EventWebApp.Core.Model;
using Microsoft.Extensions.Configuration;

namespace EventWebApp.Infrastructure.Services
{
  public class RefreshTokenService : IRefreshTokenService
  {
    private readonly ITokenService tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateRefreshTokenUseCase updateRefreshTokenUseCase;
    private readonly IConfiguration configuration;

    public RefreshTokenService(
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        UpdateRefreshTokenUseCase updateRefreshTokenUseCase,
        IConfiguration configuration)
    {
      this.tokenService = tokenService;
      this._unitOfWork = unitOfWork;
      this.updateRefreshTokenUseCase = updateRefreshTokenUseCase;
      this.configuration = configuration;
    }

    public async Task<AuthResponse> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
      var user = await _unitOfWork.Users.GetByRefreshTokenAsync(refreshToken, cancellationToken);
      if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
      {
        throw new UnauthorizedAccessException("Invalid or expired refresh token");
      }

      var newAccessToken = tokenService.GenerateAccessToken(user);

      return new AuthResponse
      {
        AccessToken = newAccessToken,
        RefreshToken = refreshToken
      };
    }

    public async Task<AuthResponse> GenerateTokensForUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
      var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
      if (user == null)
      {
        throw new ArgumentException("User not found");
      }

      var accessToken = tokenService.GenerateAccessToken(user);
      var refreshToken = tokenService.GenerateRefreshToken();
      var refreshTokenDays = configuration.GetValue<int>("JwtSettings:RefreshTokenDays", 7);
      var expiry = DateTime.UtcNow.AddDays(refreshTokenDays);

      await updateRefreshTokenUseCase.ExecuteAsync(userId, refreshToken, expiry, cancellationToken);

      return new AuthResponse
      {
        AccessToken = accessToken,
        RefreshToken = refreshToken
      };
    }

    public async Task<bool> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
      var user = await _unitOfWork.Users.GetByRefreshTokenAsync(refreshToken, cancellationToken);
      return user != null && user.RefreshTokenExpiryTime > DateTime.UtcNow;
    }

    public async Task InvalidateRefreshTokenAsync(Guid userId, CancellationToken cancellationToken = default)
    {
      await updateRefreshTokenUseCase.ExecuteAsync(userId, null, null, cancellationToken);
    }
  }
}