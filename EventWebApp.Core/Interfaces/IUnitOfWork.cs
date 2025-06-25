using EventWebApp.Core.Model;

namespace EventWebApp.Core.Interfaces
{
  public interface IUnitOfWork : IDisposable
  {
    IUserRepository Users { get; }
    IEventRepository Events { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
  }
}