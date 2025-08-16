using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestsService.Domain;
using TestsService.Domain.Shared.ValueObjects.Id;
using TestsService.Domain.ValueObjects;

namespace TestsService.Infrastructure.Configurations.Write;

public class SolvingHistoryConfiguration : IEntityTypeConfiguration<SolvingHistory>
{
    public void Configure(EntityTypeBuilder<SolvingHistory> builder)
    {
        builder.ToTable("solving_histories");
        
        builder.Property(i => i.Id)
            .HasConversion(i => i.Value, value => SolvingHistoryId.Create(value));
        
        builder.HasKey(i => i.Id);
        
        builder.Property(un => un.UniqueUserName).IsRequired();
        builder.Property(un => un.UserEmail).IsRequired();
        
        builder.Property(sh => sh.TaskHistories)
            .HasConversion(
                v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                v => JsonSerializer.Deserialize<List<TaskHistory>>(v, JsonSerializerOptions.Default)!)
            .HasColumnType("jsonb");
        
        builder.Property(sd => sd.SolvingDate).IsRequired();
        builder.Property(sts => sts.SolvingTimeSeconds).IsRequired();
    }
}