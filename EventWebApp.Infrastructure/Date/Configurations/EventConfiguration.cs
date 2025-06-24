using EventWebApp.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventWebApp.Infrastructure.Date.Configurations
{
  internal class EventConfiguration : IEntityTypeConfiguration<Event>
  {
    public void Configure(EntityTypeBuilder<Event> builder)
    {
      builder.HasKey(e => e.Id);

      builder.Property(e => e.Title).IsRequired().HasMaxLength(100);

      builder.Property(e => e.Description).IsRequired();

      builder.Property(e => e.DateTime).IsRequired();

      builder.Property(e => e.Location).IsRequired().HasMaxLength(200);

      builder.Property(e => e.Category).IsRequired().HasMaxLength(100);

      builder.Property(e => e.MaxParticipants).IsRequired();

      builder.Property(e => e.ImageUrl).HasMaxLength(500);

      builder
          .HasMany(e => e.Users)
          .WithMany(u => u.Events)
          .UsingEntity(ey => ey.ToTable("EventsUsers"));
    }
  }
}
