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

        builder.Property(un => un.Username).IsRequired().HasMaxLength(50);
        
        builder.Property(c => c.Code).IsRequired().HasMaxLength(50);
        builder.Property(ea => ea.ExpiresAt).IsRequired();
        builder.Property(a => a.Attempts).IsRequired();
    }
}