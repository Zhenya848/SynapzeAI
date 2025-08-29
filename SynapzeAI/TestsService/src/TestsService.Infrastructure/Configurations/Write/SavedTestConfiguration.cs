using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestsService.Domain;
using TestsService.Domain.Shared.ValueObjects.Id;

namespace TestsService.Infrastructure.Configurations.Write;

public class SavedTestConfiguration : IEntityTypeConfiguration<SavedTest>
{
    public void Configure(EntityTypeBuilder<SavedTest> builder)
    {
        builder.ToTable("saved_tests");
        
        builder.Property(i => i.Id)
            .HasConversion(i => i.Value, value => SavedTestId.Create(value));

        builder.Property(ui => ui.UserId);
    }
}