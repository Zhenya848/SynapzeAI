namespace TestsService.Domain.Shared.ValueObjects.Id;

public record TaskId
{
    public Guid Value { get; }
    
    public TaskId(Guid value) => Value = value;
    
    public static TaskId AddNewId() => new (Guid.NewGuid());
    
    public static TaskId AddEmptyId() => new (Guid.Empty);
    
    public static TaskId Create(Guid id) => new (id);

    public static implicit operator Guid(TaskId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        return id.Value;
    }
}