using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestsService.Domain;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

namespace TestsService.Infrastructure.Configurations.Read;

public class TaskStatisticDtoConfiguration : IEntityTypeConfiguration<TaskStatisticDto>
{
    public void Configure(EntityTypeBuilder<TaskStatisticDto> builder)
    {
        builder.ToTable("task_statistics");
        
        builder.HasKey(i => i.Id);
        
        builder.Property(ui => ui.UserId);
        
        builder.Property(ec => ec.ErrorsCount);
        builder.Property(rac => rac.RightAnswersCount);
        builder.Property(lrt => lrt.LastReviewTime);
        builder.Property(ats => ats.AvgTimeSolvingSec);
    }
}