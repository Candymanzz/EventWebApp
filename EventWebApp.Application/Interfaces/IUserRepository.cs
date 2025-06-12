using EventWebApp.Core.Model;

namespace EventWebApp.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync (Guid id);
        Task AddAsync(User user);
        Task RegisterUserToEventAsync(Guid userId, Guid eventId);
        Task CancelUserFromEvent(Guid userId, Guid eventId);
        Task<IEnumerable<User>> GetUsersByEvent(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByRefreshTokenAsync(string refreshToken);
        Task UpdateRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiry);
    }
}
