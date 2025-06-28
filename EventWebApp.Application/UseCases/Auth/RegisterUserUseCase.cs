using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;
using EventWebApp.Core.Interfaces;
using EventWebApp.Core.Model;
using Microsoft.Extensions.Configuration;

namespace EventWebApp.Application.UseCases.Auth
{
  public class RegisterDetailsUseCase
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IConfiguration _configuration;

    public RegisterDetailsUseCase(
        IUnitOfWork unitOfWork,
        ITokenService tokenService,
        IPasswordHasher passwordHasher,
        IConfiguration configuration)
    {
      _unitOfWork = unitOfWork;
      _tokenService = tokenService;
      _passwordHasher = passwordHasher;
      _configuration = configuration;
    }

    public async Task<RegisterResult> ExecuteAsync(RegisterDetailsRequest request, CancellationToken cancellationToken)
    {
      var existingUser = await _unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);
      if (existingUser != null)
      {
        return RegisterResult.Failure("User already exists.");
      }

      var refreshToken = _tokenService.GenerateRefreshToken();
      var refreshTokenDays = _configuration.GetValue<int>("JwtSettings:RefreshTokenDays", 7);

      var hashedPassword = _passwordHasher.HashPassword(request.Password);

      var newUser = new Core.Model.User
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

      return RegisterResult.Success(new AuthResponse { AccessToken = accessToken, RefreshToken = refreshToken });
    }
  }
}