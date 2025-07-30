using CSharpFunctionalExtensions;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Id;
using TestsService.Domain.ValueObjects;

namespace TestsService.Domain;

public class SolvingHistory : Shared.Entity<SolvingHistoryId>
{
    public List<TaskHistory> TaskHistories { get; } = new List<TaskHistory>();
    public DateTime SolvingDate { get; }
    public int SolvingTimeSeconds { get; }

    private SolvingHistory(SolvingHistoryId id) : base(id)
    {
        
    }
    
    private SolvingHistory(
        SolvingHistoryId id,
        IEnumerable<TaskHistory> taskHistories,
        DateTime solvingDate,
        int solvingTimeSeconds) : base(id)
    {
        TaskHistories = taskHistories.ToList();
        SolvingDate = solvingDate;
        SolvingTimeSeconds = solvingTimeSeconds;
    }

    public static Result<SolvingHistory, Error> Create(
        IEnumerable<TaskHistory> taskHistories,
        DateTime solvingDate,
        int solvingTimeSeconds)
    {
        if (solvingTimeSeconds <= 0)
            return Errors.General.ValueIsRequired(nameof(solvingDate));
        
        return new SolvingHistory(SolvingHistoryId.AddNewId(), taskHistories, solvingDate, solvingTimeSeconds);
    }
}