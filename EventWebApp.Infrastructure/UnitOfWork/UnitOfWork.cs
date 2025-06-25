using EventWebApp.Core.Interfaces;
using EventWebApp.Infrastructure.Date;
using EventWebApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace EventWebApp.Infrastructure.UnitOfWork
{
  public class UnitOfWork : IUnitOfWork
  {
    private readonly AppDbContext _context;
    private IUserRepository? _userRepository;
    private IEventRepository? _eventRepository;
    private IDbContextTransaction? _transaction;
    private bool _disposed = false;

    public UnitOfWork(AppDbContext context)
    {
      _context = context;
    }

    public IUserRepository Users
    {
      get
      {
        _userRepository ??= new UserRepository(_context);
        return _userRepository;
      }
    }

    public IEventRepository Events
    {
      get
      {
        _eventRepository ??= new EventRepository(_context);
        return _eventRepository;
      }
    }

    public async Task<int> SaveChangesAsync()
    {
      return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
      _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
      if (_transaction != null)
      {
        await _transaction.CommitAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
      }
    }

    public async Task RollbackTransactionAsync()
    {
      if (_transaction != null)
      {
        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
      }
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!_disposed && disposing)
      {
        _transaction?.Dispose();
        _context.Dispose();
      }
      _disposed = true;
    }
  }
}