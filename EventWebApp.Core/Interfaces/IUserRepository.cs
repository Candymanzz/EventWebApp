using EventWebApp.Core.Model;

namespace EventWebApp.Core.Interfaces
{
  public interface IUserRepository
  {
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User user);
    Task<RegisterUserToEventResult> RegisterUserToEventAsync(Guid userId, Guid eventId);
    Task<bool> CancelUserFromEvent(Guid userId, Guid eventId);
    Task<IEnumerable<User>> GetUsersByEvent(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByRefreshTokenAsync(string refreshToken);
    Task<bool> UpdateRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiry);
  }
}