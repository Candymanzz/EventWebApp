using EventWebApp.Core.Model;
using EventWebApp.Infrastructure.Date.Configurations;
using Microsoft.EntityFrameworkCore;

namespace EventWebApp.Infrastructure.Date
{
    public class AppDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EventConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
