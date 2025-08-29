namespace TestsService.Domain.Shared.ValueObjects.Id;

public record SolvingHistoryId
{
    public Guid Value { get; }
    
    public SolvingHistoryId(Guid value) => Value = value;
    
    public static SolvingHistoryId AddNewId() => new (Guid.NewGuid());
    
    public static SolvingHistoryId AddEmptyId() => new (Guid.Empty);
    
    public static SolvingHistoryId Create(Guid id) => new (id);

    public static implicit operator Guid(SolvingHistoryId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        return id.Value;
    }
}