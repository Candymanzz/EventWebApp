using EventWebApp.Core.Interfaces;
using EventWebApp.Core.Model;
using EventWebApp.Infrastructure.Date;
using Microsoft.EntityFrameworkCore;

namespace EventWebApp.Infrastructure.Repositories
{
  public class EventRepository : IEventRepository
  {
    private readonly AppDbContext appDbContext;

    public EventRepository(AppDbContext appDbContext)
    {
      this.appDbContext = appDbContext;
    }

    public async Task AddAsync(Event _event)
    {
      appDbContext.Events.Add(_event);
    }

    public async Task DeleteAsync(Guid id)
    {
      var _event = await appDbContext.Events.FindAsync(id);
      if (_event != null)
      {
        appDbContext.Events.Remove(_event);
      }
    }

    public async Task<IEnumerable<Event>> GetAllAsync()
    {
      return await appDbContext.Events.AsNoTracking().Include(e => e.Users).ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetByFiltersAsync(
        string? category,
        string? location,
        DateTime? dateTime,
        string? title
    )
    {
      var query = appDbContext.Events.AsNoTracking().Include(e => e.Users).AsQueryable();

      if (!string.IsNullOrEmpty(title))
      {
        Console.WriteLine($"Applying title filter: {title}");
        query = query.Where(e => e.Title.ToLower().Contains(title.ToLower()));
      }

      if (!string.IsNullOrEmpty(category))
      {
        Console.WriteLine($"Applying category filter: {category}");
        query = query.Where(e => e.Category.ToLower() == category.ToLower());
      }

      if (!string.IsNullOrEmpty(location))
      {
        Console.WriteLine($"Applying location filter: {location}");
        query = query.Where(e => e.Location.ToLower() == location.ToLower());
      }

      if (dateTime.HasValue)
      {
        Console.WriteLine($"Applying date filter: {dateTime}");
        var utcDate = DateTime.SpecifyKind(dateTime.Value.Date, DateTimeKind.Utc);
        query = query.Where(e => e.DateTime.Date == utcDate.Date);
      }

      var result = await query.ToListAsync();
      Console.WriteLine($"Found {result.Count} events after filtering");
      return result;
    }

    public async Task<Event?> GetByIdAsync(Guid id)
    {
      return await appDbContext
          .Events
          .AsNoTracking()
          .Include(e => e.Users)
          .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Event?> GetByIdForUpdateAsync(Guid id)
    {
      return await appDbContext
          .Events
          .Include(e => e.Users)
          .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Event>> GetByTitleAsync(string title)
    {
      return await appDbContext
          .Events.AsNoTracking()
          .Include(e => e.Users)
          .Where(e => e.Title.ToLower().Contains(title.ToLower()))
          .ToListAsync();
    }

    public async Task UpdateAsync(Event _event)
    {
      appDbContext.Events.Update(_event);
    }

    public async Task<PaginatedResult<Event>> GetPagedAsync(int pageNumber, int pageSize)
    {
      var query = appDbContext.Events.AsNoTracking().Include(e => e.Users).AsQueryable();

      var total = await query.CountAsync();

      var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

      return new PaginatedResult<Event>
      {
        Items = items,
        TotalCount = total,
        PageNumber = pageNumber,
        PageSize = pageSize,
      };
    }

    public async Task<List<Event>> GetEventsByUserIdAsync(Guid userId)
    {
      return await appDbContext
          .Events.AsNoTracking()
          .Include(e => e.Users)
          .Where(e => e.Users.Any(u => u.Id == userId))
          .ToListAsync();
    }

    public async Task<bool> IsUserRegisteredForEventAsync(Guid userId, Guid eventId)
    {
      return await appDbContext
          .Events
          .AsNoTracking()
          .Where(e => e.Id == eventId)
          .SelectMany(e => e.Users)
          .AnyAsync(u => u.Id == userId);
    }
  }
}
