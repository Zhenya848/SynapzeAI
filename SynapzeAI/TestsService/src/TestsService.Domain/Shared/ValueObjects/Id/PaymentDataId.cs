namespace TestsService.Domain.Shared.ValueObjects.Id;

public record PaymentDataId
{
    public Guid Value { get; }
    
    public PaymentDataId(Guid value) => Value = value;
    
    public static PaymentDataId AddNewId() => new (Guid.NewGuid());
    
    public static PaymentDataId AddEmptyId() => new (Guid.Empty);
    
    public static PaymentDataId Create(Guid id) => new (id);

    public static implicit operator Guid(PaymentDataId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        return id.Value;
    }
}