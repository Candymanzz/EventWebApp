using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;
using EventWebApp.Core.Interfaces;

namespace EventWebApp.Application.UseCases.Auth
{
  public class LoginUseCase
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IPasswordHasher _passwordHasher;

    public LoginUseCase(
        IUnitOfWork unitOfWork,
        IRefreshTokenService refreshTokenService,
        IPasswordHasher passwordHasher)
    {
      _unitOfWork = unitOfWork;
      _refreshTokenService = refreshTokenService;
      _passwordHasher = passwordHasher;
    }

    public async Task<LoginResult> ExecuteAsync(LoginRequest request, CancellationToken cancellationToken)
    {
      var user = await _unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);

      if (user == null)
      {
        return LoginResult.RequiresUserDetails(request.Email);
      }

      if (!_passwordHasher.VerifyPassword(request.Password, user.Password))
      {
        return LoginResult.Failure("Invalid email or password");
      }

      var authResponse = await _refreshTokenService.GenerateTokensForUserAsync(user.Id, cancellationToken);
      return LoginResult.Success(authResponse);
    }
  }
}