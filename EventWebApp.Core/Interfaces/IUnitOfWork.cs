using EventWebApp.Core.Model;

namespace EventWebApp.Core.Interfaces
{
  public interface IUnitOfWork : IDisposable
  {
    IUserRepository Users { get; }
    IEventRepository Events { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
  }
}