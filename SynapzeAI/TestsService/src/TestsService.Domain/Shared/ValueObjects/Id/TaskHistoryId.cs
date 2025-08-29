namespace TestsService.Domain.Shared.ValueObjects.Id;

public record TaskHistoryId
{
    public Guid Value { get; }
    
    public TaskHistoryId(Guid value) => Value = value;
    
    public static TaskHistoryId AddNewId() => new (Guid.NewGuid());
    
    public static TaskHistoryId AddEmptyId() => new (Guid.Empty);
    
    public static TaskHistoryId Create(Guid id) => new (id);

    public static implicit operator Guid(TaskHistoryId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        return id.Value;
    }
}