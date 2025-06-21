using EventWebApp.Application.Exceptions;
using EventWebApp.Application.Interfaces;
using EventWebApp.Core.Model;
using EventWebApp.Infrastructure.Date;
using Microsoft.EntityFrameworkCore;

namespace EventWebApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task AddAsync(User user)
        {
            appDbContext.Users.Add(user);
            await appDbContext.SaveChangesAsync();
        }

        public async Task CancelUserFromEvent(Guid userId, Guid eventId)
        {
            var user = await appDbContext
                .Users.Include(u => u.Events)
                .FirstOrDefaultAsync(u => u.Id == userId);
            var _event = await appDbContext
                .Events.Include(e => e.Users)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (_event == null || user == null)
            {
                throw new NotFoundException("User or Event not found.", ErrorCodes.NotFound);
            }

            if (_event.Users.Contains(user))
            {
                _event.Users.Remove(user);
            }
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await appDbContext
                .Users.AsNoTracking()
                .Include(u => u.Events)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<User>> GetUsersByEvent(Guid id)
        {
            var _event = await appDbContext
                .Events.AsNoTracking()
                .Include(e => e.Users)
                .FirstOrDefaultAsync(e => e.Id == id);

            return _event?.Users ?? Enumerable.Empty<User>();
        }

        public async Task RegisterUserToEventAsync(Guid userId, Guid eventId)
        {
            var user = await appDbContext
                .Users.Include(u => u.Events)
                .FirstOrDefaultAsync(u => u.Id == userId);
            var _event = await appDbContext
                .Events.Include(e => e.Users)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (_event == null || user == null)
            {
                throw new NotFoundException("User or Event not found.", ErrorCodes.NotFound);
            }

            if (_event.Users.Contains(user))
            {
                throw new AlreadyExistsException(
                    "User is already registered for this event.",
                    ErrorCodes.UserAlreadyRegisteredForEvent
                );
            }

            if (_event.Users.Count >= _event.MaxParticipants)
            {
                throw new ConflictException(
                    "Event has reached maximum number of participants.",
                    ErrorCodes.EventFull
                );
            }

            _event.Users.Add(user);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await appDbContext
                .Users.AsNoTracking()
                .Include(u => u.Events)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await appDbContext
                .Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task UpdateRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiry)
        {
            var user = await appDbContext.Users.FindAsync(userId);
            if (user is null)
            {
                throw new NotFoundException("User not found.", ErrorCodes.UserNotFound);
            }

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = expiry;

            await appDbContext.SaveChangesAsync();
        }
    }
}
