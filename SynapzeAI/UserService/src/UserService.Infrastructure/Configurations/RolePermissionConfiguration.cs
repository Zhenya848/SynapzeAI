using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.User;

namespace UserService.Infrastructure.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(x => new { x.RoleId, x.PermissionId });

        builder
            .HasOne(r => r.Role)
            .WithMany(rp => rp.RolePermissions)
            .HasForeignKey(ri => ri.RoleId);
        
        builder
            .HasOne(p => p.Permission)
            .WithMany()
            .HasForeignKey(pi => pi.PermissionId);
    }
}