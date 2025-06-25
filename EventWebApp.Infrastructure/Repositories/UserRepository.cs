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

    public async Task AddAsync(User user)
    {
      appDbContext.Users.Add(user);
      await appDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
      appDbContext.Users.Update(user);
      await appDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
      var user = await appDbContext.Users.FindAsync(id);
      if (user != null)
      {
        appDbContext.Users.Remove(user);
        await appDbContext.SaveChangesAsync();
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
  }
}
