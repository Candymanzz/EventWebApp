using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;
using EventWebApp.Core.Model;
using EventWebApp.Infrastructure.Date;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventWebApp.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext appDbContext;

        public EventRepository (AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task AddAsync(Event _event)
        {
            appDbContext.Events.Add(_event);
            await appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var _event = await appDbContext.Events.FindAsync(id);
            if (_event != null)
            {
                appDbContext.Events.Remove(_event);
                await appDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await appDbContext.Events.Include(e => e.Users).ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetByFiltersAsync(string? category, string? location, DateTime? dateTime)
        {
            var query = appDbContext.Events.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(e => e.Category == category);
            }

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(e => e.Location == location);
            }

            if (dateTime.HasValue)
            {
                query = query.Where(e => e.DateTime.Date == dateTime.Value.Date);
            }

            return await query.ToListAsync();
        }

        public async Task<Event?> GetByIdAsync(Guid id)
        {
            return await appDbContext.Events.Include(e => e.Users).FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Event?> GetByTitleAsync(string title)
        {
            return await appDbContext.Events.FirstOrDefaultAsync(e => e.Title == title);
        }

        public async Task UpdateAsync(Event _event)
        {
            appDbContext.Events.Update(_event);
            await appDbContext.SaveChangesAsync();
        }

        public async Task UpdateImageAsync(Guid id, string imageUrl)
        {
            var ev = await appDbContext.Events.FindAsync(id);
            if (ev == null)
                throw new InvalidOperationException("Event not found");

            ev.ImageUrl = imageUrl;
            await appDbContext.SaveChangesAsync();
        }

        public async Task<PaginatedResult<Event>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = appDbContext.Events.Include(e => e.Users).AsQueryable();

            var total = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<Event>
            {
                Items = items,
                TotalCount = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

    }
}
