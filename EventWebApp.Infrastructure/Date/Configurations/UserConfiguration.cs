using Microsoft.EntityFrameworkCore;
using EventWebApp.Core.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventWebApp.Infrastructure.Date.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(u => u.Email).IsUnique();
        }
    }
}
