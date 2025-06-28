using EventWebApp.Core.Model;

namespace EventWebApp.Core.Interfaces
{
  public interface IEventRepository
  {
    Task<IEnumerable<Event>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Event?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Event>> GetByTitleAsync(string title, CancellationToken cancellationToken = default);
    Task<IEnumerable<Event>> GetByFiltersAsync(
        string? category,
        string? location,
        DateTime? dateTime,
        string? title,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(Event _event, CancellationToken cancellationToken = default);
    Task UpdateAsync(Event _event, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PaginatedResult<Event>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<List<Event>> GetEventsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> IsUserRegisteredForEventAsync(Guid userId, Guid eventId, CancellationToken cancellationToken = default);
  }
}