using CSharpFunctionalExtensions;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Id;
using TestsService.Domain.ValueObjects;

namespace TestsService.Domain;

public class SolvingHistory : Shared.Entity<SolvingHistoryId>
{
    public string UniqueUserName { get; private set; }
    public string UserEmail { get; private set; }
    
    private List<TaskHistory> _taskHistories = new List<TaskHistory>();
    public IReadOnlyList<TaskHistory> TaskHistories => _taskHistories;
    
    public DateTime SolvingDate { get; private set; }
    public int SolvingTimeSeconds { get; private set; }

    private SolvingHistory(SolvingHistoryId id) : base(id)
    {
        
    }
    
    private SolvingHistory(
        SolvingHistoryId id,
        string uniqueUserName,
        string userEmail,
        IEnumerable<TaskHistory> taskHistories,
        DateTime solvingDate,
        int solvingTimeSeconds) : base(id)
    {
        UniqueUserName = uniqueUserName;
        UserEmail = userEmail;
        _taskHistories = taskHistories.ToList();
        SolvingDate = solvingDate;
        SolvingTimeSeconds = solvingTimeSeconds;
    }

    public static Result<SolvingHistory, Error> Create(
        string uniqueUserName,
        string userEmail,
        IEnumerable<TaskHistory> taskHistories,
        DateTime solvingDate,
        int solvingTimeSeconds)
    {
        if (string.IsNullOrWhiteSpace(uniqueUserName))
            return Errors.General.ValueIsRequired("имя пользователя");
        
        if (string.IsNullOrWhiteSpace(userEmail))
            return Errors.General.ValueIsRequired("почтовый адрес");
        
        if (solvingTimeSeconds <= 0)
            return Errors.General.ValueIsRequired(nameof(solvingDate));

        if (taskHistories.Select(sn => sn.SerialNumber).Distinct().Count() != taskHistories.Count())
            return Error.Conflict("serial.number.conflict", "Произошла дубликация серийных номеров задач");
        
        return new SolvingHistory(
            SolvingHistoryId.AddNewId(),
            uniqueUserName, 
            userEmail, 
            taskHistories, 
            solvingDate, 
            solvingTimeSeconds);
    }
}