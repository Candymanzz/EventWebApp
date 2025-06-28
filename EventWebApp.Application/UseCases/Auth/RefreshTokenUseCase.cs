using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;

namespace EventWebApp.Application.UseCases.Auth
{
  public class RefreshTokenUseCase
  {
    private readonly IRefreshTokenService _refreshTokenService;

    public RefreshTokenUseCase(IRefreshTokenService refreshTokenService)
    {
      _refreshTokenService = refreshTokenService;
    }

    public async Task<RefreshTokenResult> ExecuteAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
      try
      {
        var authResponse = await _refreshTokenService.RefreshAccessTokenAsync(request.RefreshToken, cancellationToken);
        return RefreshTokenResult.Success(authResponse);
      }
      catch (UnauthorizedAccessException)
      {
        return RefreshTokenResult.Failure("Invalid or expired refresh token");
      }
    }
  }
}