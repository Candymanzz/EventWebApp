using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;
using EventWebApp.Core.Model;
using EventWebApp.Infrastructure.Repositories;
using EventWebApp.Infrastructure.Services;
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

        public AuthController(
            ITokenService tokenService,
            IUserRepository userRepository,
            IConfiguration configuration
        )
        {
            this.tokenService = tokenService;
            this.userRepository = userRepository;
            this.configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return Ok(new { requiresDetails = true, email = request.Email });
            }

            var accessToken = tokenService.GenerateAccessToken(user);
            var refreshToken = tokenService.GenerateRefreshToken();

            return Ok(new AuthResponse { AccessToken = accessToken, RefreshToken = refreshToken });
        }

        [HttpPost("register-details")]
        public async Task<IActionResult> RegisterDetails([FromBody] RegisterDetailsRequest request)
        {
            var existingUser = await userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return BadRequest("User already exists.");
            }

            var newUser = new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = DateTime.SpecifyKind(request.BirthDate, DateTimeKind.Utc),
                RegistrationDate = DateTime.UtcNow,
                Role = string.IsNullOrWhiteSpace(request.Role) ? "User" : request.Role,
            };

            await userRepository.AddAsync(newUser);

            var accessToken = tokenService.GenerateAccessToken(newUser);
            var refreshToken = tokenService.GenerateRefreshToken();

            return Ok(new AuthResponse { AccessToken = accessToken, RefreshToken = refreshToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var user = await userRepository.GetByRefreshTokenAsync(request.RefreshToken);
            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                return Unauthorized("Invalid or expired refresh token");

            var newAccessToken = tokenService.GenerateAccessToken(user);
            var newRefreshToken = tokenService.GenerateRefreshToken();
            var expiry = DateTime.UtcNow.AddDays(int.Parse(configuration["Jwt:RefreshTokenDays"]!));

            await userRepository.UpdateRefreshTokenAsync(user.Id, newRefreshToken, expiry);

            return Ok(
                new AuthResponse { AccessToken = newAccessToken, RefreshToken = newRefreshToken }
            );
        }
    }
}
