using EventWebApp.Application.DTOs;
using EventWebApp.Core.Model;

namespace EventWebApp.Application.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(Guid id);
        Task<Event?> GetByTitleAsync(string title);
        Task<IEnumerable<Event>> GetByFiltersAsync(string? category, string? location, DateTime? dateTime);
        Task AddAsync(Event _event);
        Task UpdateAsync(Event _event);
        Task DeleteAsync(Guid id);
        Task UpdateImageAsync(Guid id, string imageUrl);
        Task<PaginatedResult<Event>> GetPagedAsync(int pageNumber, int pageSize);
    }
}
