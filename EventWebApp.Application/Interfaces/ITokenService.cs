using EventWebApp.Core.Model;

namespace EventWebApp.Application.Interfaces
{
  public interface ITokenService
  {
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
  }
}
