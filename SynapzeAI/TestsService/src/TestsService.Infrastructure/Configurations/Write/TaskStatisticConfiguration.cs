using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestsService.Domain;
using TestsService.Domain.Shared.ValueObjects.Id;

namespace TestsService.Infrastructure.Configurations.Write;

public class TaskStatisticConfiguration : IEntityTypeConfiguration<TaskStatistic>
{
    public void Configure(EntityTypeBuilder<TaskStatistic> builder)
    {
        builder.ToTable("task_statistics");
        
        builder.Property(i => i.Id).HasConversion(i => i.Value, value => TaskStatisticId.Create(value));
        builder.HasKey(i => i.Id);
        
        builder.Property(ui => ui.UserId).IsRequired();
        
        builder.Property(ec => ec.ErrorsCount).IsRequired();
        builder.Property(rac => rac.RightAnswersCount).IsRequired();
        builder.Property(lrt => lrt.LastReviewTime).IsRequired();
        builder.Property(ats => ats.AvgTimeSolvingSec).IsRequired();
    }
}