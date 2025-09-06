using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain;

namespace UserService.Infrastructure.Configurations;

public class VerificationConfiguration : IEntityTypeConfiguration<Verification>
{
    public void Configure(EntityTypeBuilder<Verification> builder)
    {
        builder.ToTable("verifications");
        builder.HasKey(i => i.Id);
        
        builder.HasOne(u => u.User).WithOne().HasForeignKey<Verification>(i => i.UserId);
        
        builder.Property(c => c.Code).IsRequired().HasMaxLength(50);
        builder.Property(ea => ea.ExpiresAt).IsRequired();
        builder.Property(a => a.Attempts).IsRequired();
    }
}