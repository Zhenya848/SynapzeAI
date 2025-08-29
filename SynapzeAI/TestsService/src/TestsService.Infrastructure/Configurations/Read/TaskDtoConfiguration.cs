using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

namespace TestsService.Infrastructure.Configurations.Read;

public class TaskDtoConfiguration : IEntityTypeConfiguration<TaskDto>
{
    public void Configure(EntityTypeBuilder<TaskDto> builder)
    {
        builder.ToTable("tasks");
        
        builder.HasKey(i => i.Id);

        builder.Property(ti => ti.TestId);
        
        builder.Property(sn => sn.SerialNumber);
        builder.Property(tn => tn.TaskName);
        builder.Property(tm => tm.TaskMessage);
        builder.Property(ra => ra.RightAnswer).IsRequired(false);
        
        builder.HasMany(ts => ts.TaskStatistics).WithOne().HasForeignKey(i => i.TaskId).OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(a => a.Answers).HasConversion(
                value => JsonSerializer.Serialize(value, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<string[]>(json, JsonSerializerOptions.Default)!)
            .HasColumnType("jsonb")
            .HasColumnName("answers")
            .IsRequired(false);
    }
}