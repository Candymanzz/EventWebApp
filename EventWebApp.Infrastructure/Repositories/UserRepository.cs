using EventWebApp.Core.Interfaces;
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

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
      appDbContext.Users.Add(user);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
      appDbContext.Users.Update(user);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
      var user = await appDbContext.Users.FindAsync(new object[] { id }, cancellationToken);
      if (user != null)
      {
        appDbContext.Users.Remove(user);
      }
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
      return await appDbContext
          .Users
          .AsNoTracking()
          .Include(u => u.Events)
          .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default)
    {
      return await appDbContext
          .Users
          .Include(u => u.Events)
          .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetUsersByEvent(Guid id, CancellationToken cancellationToken = default)
    {
      var _event = await appDbContext
          .Events.AsNoTracking()
          .Include(e => e.Users)
          .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

      return _event?.Users ?? Enumerable.Empty<User>();
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
      return await appDbContext
          .Users.AsNoTracking()
          .Include(u => u.Events)
          .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
      return await appDbContext
          .Users.AsNoTracking()
          .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, cancellationToken);
    }
  }
}
