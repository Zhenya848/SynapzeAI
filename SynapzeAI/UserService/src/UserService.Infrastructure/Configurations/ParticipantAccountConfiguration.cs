using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Shared;
using UserService.Domain.User;

namespace UserService.Infrastructure.Configurations;

public class ParticipantAccountConfiguration : IEntityTypeConfiguration<ParticipantAccount>
{
    public void Configure(EntityTypeBuilder<ParticipantAccount> builder)
    {
        builder.ToTable("participant_accounts");
        builder.HasKey(i => i.Id);
        
        builder.HasOne(u => u.User)
            .WithOne(p => p.ParticipantAccount)
            .HasForeignKey<ParticipantAccount>(i => i.UserId);
       
        builder.Property(nn => nn.Nickname).IsRequired().HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
    }
}