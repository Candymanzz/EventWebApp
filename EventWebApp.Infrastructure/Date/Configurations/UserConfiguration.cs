using EventWebApp.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventWebApp.Infrastructure.Date.Configurations
{
  internal class UserConfiguration : IEntityTypeConfiguration<User>
  {
    public void Configure(EntityTypeBuilder<User> builder)
    {
      builder.HasKey(u => u.Id);

      builder.Property(u => u.FirstName).IsRequired().HasMaxLength(100);

      builder.Property(u => u.LastName).IsRequired().HasMaxLength(100);

      builder.Property(u => u.Email).IsRequired().HasMaxLength(200);

      builder.HasIndex(u => u.Email).IsUnique();

      builder.Property(u => u.DateOfBirth).IsRequired();

      builder.Property(u => u.RegistrationDate).IsRequired();

      builder.Property(u => u.Password).IsRequired().HasMaxLength(255);

      builder.Property(u => u.Role).IsRequired().HasMaxLength(10);

      builder.Property(u => u.RefreshToken).HasMaxLength(200);

      builder.Property(u => u.RefreshTokenExpiryTime);

      builder.HasMany(u => u.Events).WithMany(e => e.Users);
    }
  }
}
