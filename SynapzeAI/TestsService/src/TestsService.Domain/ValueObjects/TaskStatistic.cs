using CSharpFunctionalExtensions;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Dtos;

namespace TestsService.Domain.ValueObjects;

public record TaskStatistic
{
    public int ErrorsCount { get; }
    public int RightAnswersCount { get; }
    public DateTime LastReviewTime { get; }
    public float AvgTimeSolvingSec { get; }
    
    private TaskStatistic(
        int errorsCount,
        int rightAnswersCount,
        DateTime lastReviewTime,
        float avgTimeSolvingSec)
    {
        ErrorsCount = errorsCount;
        RightAnswersCount = rightAnswersCount;
        LastReviewTime = lastReviewTime;
        AvgTimeSolvingSec = avgTimeSolvingSec;
    }
    
    public static Result<TaskStatistic, Error> Create(
        int errorsCount, 
        int rightAnswersCount, 
        DateTime lastReviewTime,
        float avgTimeSolvingSec)
    {
        if (errorsCount < 0)
            return Errors.General.ValueIsInvalid(nameof(errorsCount));
        
        if (rightAnswersCount < 0)
            return Errors.General.ValueIsInvalid(nameof(rightAnswersCount));
        
        if (lastReviewTime > DateTime.Now)
            return Errors.General.ValueIsInvalid(nameof(lastReviewTime));
        
        if (avgTimeSolvingSec <= 0)
            return Errors.General.ValueIsInvalid(nameof(avgTimeSolvingSec));
        
        return new TaskStatistic(
            errorsCount, 
            rightAnswersCount, 
            lastReviewTime, 
            avgTimeSolvingSec);
    }
}