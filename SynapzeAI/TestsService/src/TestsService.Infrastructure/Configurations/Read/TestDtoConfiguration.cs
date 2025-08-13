using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

namespace TestsService.Infrastructure.Configurations.Read;

public class TestDtoConfiguration : IEntityTypeConfiguration<TestDto>
{
    public void Configure(EntityTypeBuilder<TestDto> builder)
    {
        builder.ToTable("tests");
        
        builder.HasKey(i => i.Id);
        
        builder.Property(tn => tn.TestName);
        builder.Property(t => t.Theme);
        builder.Property(wa => wa.WithAI);

        builder.Property(ui => ui.UserId);

        builder.OwnsOne(lt => lt.LimitTime, ltb =>
        {
            ltb.Property(s => s.Seconds).IsRequired().HasColumnName("seconds");
            ltb.Property(m => m.Minutes).IsRequired().HasColumnName("minutes");
        });
        
        builder.OwnsOne(ps => ps.PrivacySettings, psb =>
        {
            psb.Property(ip => ip.IsPrivate).HasColumnName("is_private");

            psb.Property(u => u.UsersNamesAreAllowed).HasConversion(
                    value => JsonSerializer.Serialize(value, JsonSerializerOptions.Default),
                    json => JsonSerializer.Deserialize<string[]>(json, JsonSerializerOptions.Default)!)
                .HasColumnType("jsonb")
                .HasColumnName("users_names_are_allowed")
                .IsRequired(false);

            psb.Property(u => u.UsersEmailsAreAllowed).HasConversion(
                    value => JsonSerializer.Serialize(value, JsonSerializerOptions.Default),
                    json => JsonSerializer.Deserialize<string[]>(json, JsonSerializerOptions.Default)!)
                .HasColumnType("jsonb")
                .HasColumnName("users_emails_are_allowed")
                .IsRequired(false);
        });
        
        builder.Navigation(lt => lt.LimitTime).IsRequired(false);
    }
}