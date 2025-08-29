using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

namespace TestsService.Infrastructure.Configurations.Read;

public class SavedTestDtoConfiguration : IEntityTypeConfiguration<SavedTestDto>
{
    public void Configure(EntityTypeBuilder<SavedTestDto> builder)
    {
        builder.ToTable("saved_tests");
        
        builder.Property(i => i.Id);

        builder.Property(ui => ui.UserId);
    }
}