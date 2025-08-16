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
        builder.Property(ui => ui.UserId);
        
        builder.Property(u => u.UniqueUserName);
        
        builder.Property(tn => tn.TestName);
        builder.Property(t => t.Theme);
        builder.Property(ip => ip.IsPublished);

        builder.OwnsOne(lt => lt.LimitTime, ltb =>
        {
            ltb.Property(s => s.Seconds).IsRequired().HasColumnName("seconds");
            ltb.Property(m => m.Minutes).IsRequired().HasColumnName("minutes");
        });
        
        builder.Navigation(lt => lt.LimitTime).IsRequired(false);
    }
}