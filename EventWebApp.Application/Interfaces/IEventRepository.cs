using EventWebApp.Application.DTOs;
using EventWebApp.Core.Model;

namespace EventWebApp.Application.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(Guid id);
        Task<IEnumerable<Event>> GetByTitleAsync(string title);
        Task<IEnumerable<Event>> GetByFiltersAsync(
            string? category,
            string? location,
            DateTime? dateTime,
            string? title
        );
        Task AddAsync(Event _event);
        Task UpdateAsync(Event _event);
        Task DeleteAsync(Guid id);
        Task UpdateImageAsync(Guid id, string imageUrl);
        Task<PaginatedResult<Event>> GetPagedAsync(int pageNumber, int pageSize);
        Task<List<Event>> GetEventsByUserIdAsync(Guid userId);
    }
}
