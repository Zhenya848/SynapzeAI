using Core;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Id;

namespace TestsService.Domain;

public class SavedTest : Entity<SavedTestId>
{
    public TestId TestId { get; init; }
    public Guid UserId { get; private set; }
    

    private SavedTest(SavedTestId id) : base(id)
    {
        
    }

    public SavedTest(SavedTestId id, Guid userId) : base(id)
    {
        UserId = userId;
    }
}