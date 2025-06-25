using EventWebApp.Core.Model;

namespace EventWebApp.Core.Interfaces
{
  public interface IUserRepository
  {
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<User>> GetUsersByEvent(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByRefreshTokenAsync(string refreshToken);
  }
}