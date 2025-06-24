using EventWebApp.Core.Model;
using EventWebApp.Infrastructure.Date.Configurations;
using Microsoft.EntityFrameworkCore;

namespace EventWebApp.Infrastructure.Date
{
  public class AppDbContext : DbContext
  {
    public DbSet<Event> Events { get; set; }
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions)
        : base(dbContextOptions) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfiguration(new EventConfiguration());
      modelBuilder.ApplyConfiguration(new UserConfiguration());

      base.OnModelCreating(modelBuilder);
    }
  }
}
