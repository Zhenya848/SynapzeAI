using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestsService.Domain;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Id;

namespace TestsService.Infrastructure.Configurations.Write;

public class TestConfiguration : IEntityTypeConfiguration<Test>
{
    public void Configure(EntityTypeBuilder<Test> builder)
    {
        builder.ToTable("tests");

        builder.Property(i => i.Id).HasConversion(i => i.Value, value => TestId.Create(value));
        builder.HasKey(i => i.Id);
        
        builder.Property(ui => ui.UserId).IsRequired();
        
        builder.Property(tn => tn.TestName).IsRequired().HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        builder.Property(t => t.Theme).IsRequired().HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        builder.Property(wa => wa.WithAI).IsRequired();

        builder.OwnsOne(lt => lt.LimitTime, ltb =>
        {
            ltb.Property(s => s.Seconds).HasColumnName("seconds");
            ltb.Property(m => m.Minutes).HasColumnName("minutes");
        });

        builder.OwnsOne(ps => ps.PrivacySettings, psb =>
        {
            psb.Property(ip => ip.IsPrivate).HasColumnName("is_private");

            psb.Property(u => u.UsersNamesAreAllowed).HasConversion(
                value => JsonSerializer.Serialize(value, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<List<string>>(json, JsonSerializerOptions.Default)!)
            .HasColumnType("jsonb")
            .HasColumnName("users_names_are_allowed")
            .IsRequired(false);

            psb.Property(u => u.UsersEmailsAreAllowed).HasConversion(
                value => JsonSerializer.Serialize(value, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<List<string>>(json, JsonSerializerOptions.Default)!)
            .HasColumnType("jsonb")
            .HasColumnName("users_emails_are_allowed")
            .IsRequired(false);
        });
        
        builder.Navigation(lt => lt.LimitTime).IsRequired(false);
        
        builder.HasMany(t => t.Tasks).WithOne().HasForeignKey("test_id").OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(sh => sh.SolvingHistories).WithOne().HasForeignKey("test_id").OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(idlt => idlt.IsDeleted);
        builder.Property(dd => dd.DeletionDate);
    }
}