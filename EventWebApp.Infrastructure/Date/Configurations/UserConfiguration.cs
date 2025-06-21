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

            builder.Property(u => u.FirstName).IsRequired().HasMaxLength(50);

            builder.Property(u => u.LastName).IsRequired().HasMaxLength(50);

            builder.Property(u => u.Email).IsRequired().HasMaxLength(100);

            builder.HasIndex(u => u.Email).IsUnique(); //

            builder.Property(u => u.DateOfBirth).IsRequired();

            builder.Property(u => u.RegistrationDate).IsRequired();

            builder.Property(u => u.Role).IsRequired().HasMaxLength(20);

            builder.Property(u => u.RefreshToken).HasMaxLength(200);

            builder.Property(u => u.RefreshTokenExpiryTime);

            builder.HasIndex(u => u.Email).IsUnique();

            builder.HasMany(u => u.Events).WithMany(e => e.Users);
        }
    }
}
