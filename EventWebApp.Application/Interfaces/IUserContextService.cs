using System.Security.Claims;

namespace EventWebApp.Application.Interfaces
{
  public interface IUserContextService
  {
    Guid? GetUserId(ClaimsPrincipal user);
  }
}