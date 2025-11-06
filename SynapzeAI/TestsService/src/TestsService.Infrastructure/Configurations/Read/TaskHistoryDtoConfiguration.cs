using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestsService.Domain.Shared.ValueObjects.Dtos;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;
using Constants = TestsService.Domain.Shared.Constants;

namespace TestsService.Infrastructure.Configurations.Read;

public class TaskHistoryDtoConfiguration : IEntityTypeConfiguration<TaskHistoryDto>
{
    public void Configure(EntityTypeBuilder<TaskHistoryDto> builder)
    {
        builder.ToTable("task_histories");
        
        builder.HasKey(i => i.Id);
        
        builder.Property(sn => sn.SerialNumber);
        builder.Property(tn => tn.TaskName);
        builder.Property(tm => tm.TaskMessage);
        builder.Property(ra => ra.RightAnswer);
        
        builder.Property(a => a.Answers).HasConversion(
                value => JsonSerializer.Serialize(value, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<string[]>(json, JsonSerializerOptions.Default)!)
            .HasColumnType("jsonb")
            .HasColumnName("answers")
            .IsRequired(false);
        
        builder.Property(mai => mai.Message).IsRequired(false).HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
        builder.Property(p => p.Points).IsRequired(false);
    }
}