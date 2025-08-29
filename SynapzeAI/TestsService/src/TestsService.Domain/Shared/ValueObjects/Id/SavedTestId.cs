namespace TestsService.Domain.Shared.ValueObjects.Id;

public record SavedTestId
{
    public Guid Value { get; }
    
    public SavedTestId(Guid value) => Value = value;
    
    public static SavedTestId AddNewId() => new (Guid.NewGuid());
    
    public static SavedTestId AddEmptyId() => new (Guid.Empty);
    
    public static SavedTestId Create(Guid id) => new (id);

    public static implicit operator Guid(SavedTestId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        return id.Value;
    }
}