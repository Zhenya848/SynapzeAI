using CSharpFunctionalExtensions;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Id;
using TestsService.Domain.ValueObjects;

namespace TestsService.Domain;

public class SolvingHistory : Shared.Entity<SolvingHistoryId>
{
    public List<TaskHistory> TaskHistories { get; private set; } = new List<TaskHistory>();
    public DateTime SolvingDate { get; private set; }
    public int SolvingTimeSeconds { get; private set; }

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

        if (taskHistories.Select(sn => sn.SerialNumber).Distinct().Count() != taskHistories.Count())
            return Error.Conflict("serial.number.conflict", "Some serial numbers are duplicated");
        
        return new SolvingHistory(SolvingHistoryId.AddNewId(), taskHistories, solvingDate, solvingTimeSeconds);
    }

    public UnitResult<Error> UpdateInfo(
        IEnumerable<TaskHistory> taskHistories,
        DateTime solvingDate,
        int solvingTimeSeconds)
    {
        if (solvingTimeSeconds <= 0)
            return Errors.General.ValueIsRequired(nameof(solvingDate));

        if (taskHistories.Select(sn => sn.SerialNumber).Distinct().Count() != taskHistories.Count())
            return Error.Conflict("serial.number.conflict", "Some serial numbers are duplicated");
        
        TaskHistories = taskHistories.ToList();
        SolvingDate = solvingDate;
        SolvingTimeSeconds =  solvingTimeSeconds;
        
        return Result.Success<Error>();
    }
}