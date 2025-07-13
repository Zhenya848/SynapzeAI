using CSharpFunctionalExtensions;
using TestsService.Domain.Shared;

namespace TestsService.Domain.ValueObjects;

public record LimitTime
{
    public int Seconds { get; }
    public int Minutes { get; }

    private LimitTime(int seconds, int minutes)
    {
        Seconds = seconds;
        Minutes = minutes;
    }
    
    public static Result<LimitTime, Error> Create(int seconds, int minutes)
    {
        long allSeconds = seconds + minutes * 60;
        
        if (allSeconds < 10)
            return Errors.General.ValueIsInvalid("Limit time must be at least 10.");
        
        int minutesResult = (int)(allSeconds / 60);
        int secondsResult = (int)(allSeconds % 60);
        
        return new LimitTime(secondsResult, minutesResult);
    }
}