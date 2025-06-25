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
    private readonly IUserRepository userRepository;
    private readonly IConfiguration configuration;
    private readonly IRefreshTokenService refreshTokenService;

    public AuthController(
        ITokenService tokenService,
        IUserRepository userRepository,
        IConfiguration configuration,
        IRefreshTokenService refreshTokenService
    )
    {
      this.tokenService = tokenService;
      this.userRepository = userRepository;
      this.configuration = configuration;
      this.refreshTokenService = refreshTokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
      var user = await userRepository.GetByEmailAsync(request.Email);
      if (user == null)
      {
        return Ok(new { requiresDetails = true, email = request.Email });
      }

      if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
      {
        return BadRequest("Invalid email or password");
      }

      var authResponse = await refreshTokenService.GenerateTokensForUserAsync(user.Id);

      return Ok(authResponse);
    }

    [HttpPost("register-details")]
    public async Task<IActionResult> RegisterDetails([FromBody] RegisterDetailsRequest request)
    {
      var existingUser = await userRepository.GetByEmailAsync(request.Email);
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

      await userRepository.AddAsync(newUser);

      var accessToken = tokenService.GenerateAccessToken(newUser);

      return Ok(new AuthResponse { AccessToken = accessToken, RefreshToken = refreshToken });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
      try
      {
        var authResponse = await refreshTokenService.RefreshAccessTokenAsync(request.RefreshToken);
        return Ok(authResponse);
      }
      catch (UnauthorizedAccessException)
      {
        return Unauthorized("Invalid or expired refresh token");
      }
    }
  }
}
