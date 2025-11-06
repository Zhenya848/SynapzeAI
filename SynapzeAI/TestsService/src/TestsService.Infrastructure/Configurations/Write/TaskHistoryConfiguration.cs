using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.VisualBasic;
using TestsService.Domain;
using TestsService.Domain.Shared.ValueObjects.Id;
using Constants = TestsService.Domain.Shared.Constants;

namespace TestsService.Infrastructure.Configurations.Write;

public class TaskHistoryConfiguration : IEntityTypeConfiguration<TaskHistory>
{
    public void Configure(EntityTypeBuilder<TaskHistory> builder)
    {
        builder.ToTable("task_histories");
        
        builder.Property(i => i.Id)
            .HasConversion(i => i.Value, value => TaskHistoryId.Create(value));
        
        builder.HasKey(i => i.Id);
        
        builder.Property(sn => sn.SerialNumber).IsRequired();
        builder.Property(tn => tn.TaskName).IsRequired().HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        builder.Property(tm => tm.TaskMessage).IsRequired().HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
        builder.Property(ra => ra.RightAnswer).IsRequired(false).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        
        builder.Property(a => a.Answers).HasConversion(
                value => JsonSerializer.Serialize(value, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<List<string>>(json, JsonSerializerOptions.Default)!)
            .HasColumnType("jsonb")
            .HasColumnName("answers")
            .IsRequired(false);
        
        builder.Property(mai => mai.Message).IsRequired(false).HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
        builder.Property(p => p.Points).IsRequired(false);
    }
}