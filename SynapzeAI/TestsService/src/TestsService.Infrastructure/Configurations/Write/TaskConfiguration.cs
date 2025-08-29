using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Id;
using TestsService.Domain.ValueObjects;
using Task = TestsService.Domain.Task;

namespace TestsService.Infrastructure.Configurations.Write;

public class TaskConfiguration : IEntityTypeConfiguration<Task>
{
    public void Configure(EntityTypeBuilder<Task> builder)
    {
        builder.ToTable("tasks");

        builder.Property(i => i.Id).HasConversion(i => i.Value, value => TaskId.Create(value));
        builder.HasKey(i => i.Id);
        
        builder.Property(sn => sn.SerialNumber).IsRequired();
        builder.Property(tn => tn.TaskName).IsRequired().HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        builder.Property(tm => tm.TaskMessage).IsRequired().HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
        builder.Property(ra => ra.RightAnswer).IsRequired(false).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        
        builder.HasMany(ts => ts.TaskStatistics).WithOne().HasForeignKey("task_id").OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(a => a.Answers).HasConversion(
                value => JsonSerializer.Serialize(value, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<List<string>>(json, JsonSerializerOptions.Default)!)
            .HasColumnType("jsonb")
            .HasColumnName("answers")
            .IsRequired(false);
    }
}