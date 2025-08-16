using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestsService.Domain.Shared.ValueObjects.Dtos;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;
using TestsService.Domain.ValueObjects;

namespace TestsService.Infrastructure.Configurations.Read;

public class SolvingHistoryDtoConfiguration : IEntityTypeConfiguration<SolvingHistoryDto>
{
    public void Configure(EntityTypeBuilder<SolvingHistoryDto> builder)
    {
        builder.ToTable("solving_histories");
        
        builder.HasKey(i => i.Id);

        builder.Property(ti => ti.TestId);
        
        builder.Property(un => un.UniqueUserName);
        builder.Property(un => un.UserEmail);
        
        builder.Property(sh => sh.TaskHistories)
            .HasConversion(
                v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                v => JsonSerializer.Deserialize<TaskHistoryDto[]>(v, JsonSerializerOptions.Default)!)
            .HasColumnType("jsonb");
        
        builder.Property(sd => sd.SolvingDate);
        builder.Property(sts => sts.SolvingTimeSeconds);
    }
}