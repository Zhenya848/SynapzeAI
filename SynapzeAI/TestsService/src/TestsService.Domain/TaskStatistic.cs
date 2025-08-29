using CSharpFunctionalExtensions;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Id;

namespace TestsService.Domain;

public class TaskStatistic : Shared.Entity<TaskStatisticId>
{
    public Guid UserId { get; private set; }
    
    public int ErrorsCount { get; private set; }
    public int RightAnswersCount { get; private set; }
    public DateTime LastReviewTime { get; private set; }
    public float AvgTimeSolvingSec { get; private set; }

    private TaskStatistic(TaskStatisticId id) : base(id)
    {
        
    }
    
    private TaskStatistic(
        TaskStatisticId id,
        Guid userId,
        int errorsCount,
        int rightAnswersCount,
        DateTime lastReviewTime,
        float avgTimeSolvingSec) : base(id)
    {
        UserId = userId;
        ErrorsCount = errorsCount;
        RightAnswersCount = rightAnswersCount;
        LastReviewTime = lastReviewTime;
        AvgTimeSolvingSec = avgTimeSolvingSec;
    }
    
    public static Result<TaskStatistic, Error> Create(
        Guid userId,
        int errorsCount, 
        int rightAnswersCount, 
        DateTime lastReviewTime,
        float avgTimeSolvingSec)
    {
        if (errorsCount < 0)
            return Errors.General.ValueIsInvalid("количество неверных ответов");
        
        if (rightAnswersCount < 0)
            return Errors.General.ValueIsInvalid("количество верных ответов");
        
        if (lastReviewTime > DateTime.Now)
            return Errors.General.ValueIsInvalid("последнее время решения");
        
        if (avgTimeSolvingSec < 0)
            return Errors.General.ValueIsInvalid("среднее время решения задачи");
        
        return new TaskStatistic(
            TaskStatisticId.AddNewId(),
            userId,
            errorsCount, 
            rightAnswersCount, 
            lastReviewTime, 
            avgTimeSolvingSec);
    }
    
    public UnitResult<Error> Update(
        int errorsCount, 
        int rightAnswersCount, 
        DateTime lastReviewTime,
        float avgTimeSolvingSec)
    {
        if (errorsCount < 0)
            return Errors.General.ValueIsInvalid("количество неверных ответов");
        
        if (rightAnswersCount < 0)
            return Errors.General.ValueIsInvalid("количество верных ответов");
        
        if (lastReviewTime > DateTime.Now)
            return Errors.General.ValueIsInvalid("последнее время решения");
        
        if (avgTimeSolvingSec < 0)
            return Errors.General.ValueIsInvalid("среднее время решения задачи");
        
        ErrorsCount = errorsCount;
        RightAnswersCount = rightAnswersCount;
        LastReviewTime = lastReviewTime;
        AvgTimeSolvingSec = avgTimeSolvingSec;

        return Result.Success<Error>();
    }
}