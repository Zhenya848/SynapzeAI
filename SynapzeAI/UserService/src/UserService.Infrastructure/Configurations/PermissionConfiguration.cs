using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Shared;
using UserService.Domain.User;

namespace UserService.Infrastructure.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");
        
        builder.HasIndex(c => c.Code).IsUnique();
        builder.Property(d => d.Description).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
    }
}