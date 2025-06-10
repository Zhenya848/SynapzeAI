using CSharpFunctionalExtensions;
using TestsService.Domain.Shared;

namespace TestsService.Domain.ValueObjects;

public record PriorityNumber
{
    public float Value { get; private set; }
    
    private PriorityNumber(float value) =>
        Value = value;

    public static Result<PriorityNumber, Error> Create(float priorityValue = 1)
    {
        if (priorityValue <= 0)
            return Error.Validation("priority_value.is.invalid", "priority value must be greater than 0");
        
        return new PriorityNumber(priorityValue);
    }
    
    public void Update(float waitingTime, int rightAnswers, int wrongAnswers) =>
        Value = waitingTime / (Value * (0.5f * rightAnswers - 0.3f * wrongAnswers));
}