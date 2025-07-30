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
            ltb.Property(s => s.Seconds);
            ltb.Property(m => m.Minutes);
        });
        
        builder.Navigation(lt => lt.LimitTime).IsRequired(false);
        
        builder.HasMany(t => t.Tasks).WithOne().HasForeignKey("test_id").OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(sh => sh.SolvingHistories).WithOne().HasForeignKey("test_id").OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(idlt => idlt.IsDeleted);
        builder.Property(dd => dd.DeletionDate);
    }
}