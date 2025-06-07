using Microsoft.EntityFrameworkCore;
using EventWebApp.Core.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventWebApp.Infrastructure.Date.Configurations
{
    internal class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasMany(e => e.Users).WithOne(u => u.Event).HasForeignKey(u => u.EventId);
        }
    }
}