using BCrypt.Net;
using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;
using EventWebApp.Core.Interfaces;
using EventWebApp.Core.Model;
using Microsoft.AspNetCore.Mvc;

namespace EventWebApp.WebAPI.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly ITokenService tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration configuration;
    private readonly IRefreshTokenService refreshTokenService;

    public AuthController(
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        IRefreshTokenService refreshTokenService
    )
    {
      this.tokenService = tokenService;
      this._unitOfWork = unitOfWork;
      this.configuration = configuration;
      this.refreshTokenService = refreshTokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
      var user = await _unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);
      if (user == null)
      {
        return Ok(new { requiresDetails = true, email = request.Email });
      }

      if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
      {
        return BadRequest("Invalid email or password");
      }

      var authResponse = await refreshTokenService.GenerateTokensForUserAsync(user.Id, cancellationToken);

      return Ok(authResponse);
    }

    [HttpPost("register-details")]
    public async Task<IActionResult> RegisterDetails([FromBody] RegisterDetailsRequest request, CancellationToken cancellationToken)
    {
      var existingUser = await _unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);
      if (existingUser != null)
      {
        return BadRequest("User already exists.");
      }

      var refreshToken = tokenService.GenerateRefreshToken();
      var refreshTokenDays = configuration.GetValue<int>("JwtSettings:RefreshTokenDays", 7);

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

      var accessToken = tokenService.GenerateAccessToken(newUser);

      return Ok(new AuthResponse { AccessToken = accessToken, RefreshToken = refreshToken });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
      try
      {
        var authResponse = await refreshTokenService.RefreshAccessTokenAsync(request.RefreshToken, cancellationToken);
        return Ok(authResponse);
      }
      catch (UnauthorizedAccessException)
      {
        return Unauthorized("Invalid or expired refresh token");
      }
    }
  }
}
