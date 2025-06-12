using EventWebApp.Core.Model;
using EventWebApp.Infrastructure.Date;
using EventWebApp.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventWebApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext appDbContext;

        public UserRepository (AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task AddAsync(User user)
        {
            appDbContext.Users.Add(user); // AddAsync????
            await appDbContext.SaveChangesAsync();
        }

        public async Task CancelUserFromEvent(Guid userId, Guid eventId)
        {
            var user = await appDbContext.Users.Include(u => u.Events).FirstOrDefaultAsync(u => u.Id == userId);
            var _event = await appDbContext.Events.Include(e => e.Users).FirstOrDefaultAsync(e => e.Id == eventId);

            if (_event == null || user == null)
            {
                throw new InvalidOperationException("User or Event not found.");
            }

            if (_event.Users.Contains(user))
            {
                _event.Users.Remove(user);
            }
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await appDbContext.Users.Include(u => u.Events).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<User>> GetUsersByEvent(Guid id)
        {
            var _event = await appDbContext.Events.Include(e => e.Users).FirstOrDefaultAsync(e => e.Id == id);

            return _event?.Users ?? Enumerable.Empty<User>(); //null
        }

        public async Task RegisterUserToEventAsync(Guid userId, Guid eventId)
        {
            var user = await appDbContext.Users.Include(u => u.Events).FirstOrDefaultAsync(u => u.Id == userId);
            var _event = await appDbContext.Events.Include(e => e.Users).FirstOrDefaultAsync(e => e.Id == eventId);

            if (_event == null || user == null)
            {
                throw new InvalidOperationException("User or Event not found.");
            }

            if (!_event.Users.Contains(user))
            {
                _event.Users.Add(user);
            }

            await appDbContext.SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await appDbContext.Users
                .Include(u => u.Events)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await appDbContext.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task UpdateRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiry)
        {
            var user = await appDbContext.Users.FindAsync(userId);
            if (user is null) return;

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = expiry;

            await appDbContext.SaveChangesAsync();
        }

    }
}
