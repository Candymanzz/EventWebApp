using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EventWebApp.Application.Interfaces;
using EventWebApp.Core.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EventWebApp.Infrastructure.Services
{
  public class TokenService : ITokenService
  {
    private readonly IConfiguration configuration;

    public TokenService(IConfiguration configuration)
    {
      this.configuration = configuration;
    }

    public string GenerateAccessToken(User user)
    {
      var claims = new[]
      {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                new Claim(ClaimTypes.Role, user.Role),
            };

      var key = new SymmetricSecurityKey(
          Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]!)
      );
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
          issuer: configuration["JwtSettings:Issuer"],
          audience: configuration["JwtSettings:Audience"],
          claims: claims,
          expires: DateTime.UtcNow.AddMinutes(
              configuration.GetValue<int>("JwtSettings:ExpirationMinutes", 60)
          ),
          signingCredentials: creds
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
      return Guid.NewGuid().ToString() + Guid.NewGuid();
    }
  }
}
