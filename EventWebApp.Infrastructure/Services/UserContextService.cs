using EventWebApp.Application.Interfaces;
using System.Security.Claims;

namespace EventWebApp.Infrastructure.Services
{
  public class UserContextService : IUserContextService
  {
    public Guid? GetUserId(ClaimsPrincipal user)
    {
      var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
      if (userId == null)
      {
        return null;
      }

      if (Guid.TryParse(userId, out var parsedUserId))
      {
        return parsedUserId;
      }

      return null;
    }
  }
}