namespace TestsService.Domain.Shared.ValueObjects.Id;

public record TaskStatisticId
{
    public Guid Value { get; }
    
    public TaskStatisticId(Guid value) => Value = value;
    
    public static TaskStatisticId AddNewId() => new (Guid.NewGuid());
    
    public static TaskStatisticId AddEmptyId() => new (Guid.Empty);
    
    public static TaskStatisticId Create(Guid id) => new (id);

    public static implicit operator Guid(TaskStatisticId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        return id.Value;
    }
}