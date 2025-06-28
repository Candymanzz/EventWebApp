using BCrypt.Net;
using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;
using EventWebApp.Core.Interfaces;
using EventWebApp.Core.Model;

namespace EventWebApp.WebAPI.Services
{
  public class AuthValidationService : IAuthValidationService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IConfiguration _configuration;

    public AuthValidationService(
        IUnitOfWork unitOfWork,
        ITokenService tokenService,
        IRefreshTokenService refreshTokenService,
        IConfiguration configuration)
    {
      _unitOfWork = unitOfWork;
      _tokenService = tokenService;
      _refreshTokenService = refreshTokenService;
      _configuration = configuration;
    }

    public async Task<(bool isValid, object? result)> ValidateLoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
      var user = await _unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);
      if (user == null)
      {
        return (false, new { requiresDetails = true, email = request.Email });
      }

      if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
      {
        return (false, "Invalid email or password");
      }

      var authResponse = await _refreshTokenService.GenerateTokensForUserAsync(user.Id, cancellationToken);
      return (true, authResponse);
    }

    public async Task<(bool isValid, object? result)> ValidateRegisterDetailsAsync(RegisterDetailsRequest request, CancellationToken cancellationToken)
    {
      var existingUser = await _unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);
      if (existingUser != null)
      {
        return (false, "User already exists.");
      }

      var refreshToken = _tokenService.GenerateRefreshToken();
      var refreshTokenDays = _configuration.GetValue<int>("JwtSettings:RefreshTokenDays", 7);

      var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

      var newUser = new User
      {
        Email = request.Email,
        Password = hashedPassword,
        FirstName = request.FirstName,
        LastName = request.LastName,
        DateOfBirth = DateTime.SpecifyKind(request.BirthDate, DateTimeKind.Utc),
        RegistrationDate = DateTime.UtcNow,
        Role = string.IsNullOrWhiteSpace(request.Role) ? "User" : request.Role,
        RefreshToken = refreshToken,
        RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenDays),
      };

      await _unitOfWork.Users.AddAsync(newUser, cancellationToken);
      await _unitOfWork.SaveChangesAsync(cancellationToken);

      var accessToken = _tokenService.GenerateAccessToken(newUser);

      return (true, new AuthResponse { AccessToken = accessToken, RefreshToken = refreshToken });
    }

    public async Task<(bool isValid, object? result)> ValidateRefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
      try
      {
        var authResponse = await _refreshTokenService.RefreshAccessTokenAsync(request.RefreshToken, cancellationToken);
        return (true, authResponse);
      }
      catch (UnauthorizedAccessException)
      {
        return (false, "Invalid or expired refresh token");
      }
    }
  }
}