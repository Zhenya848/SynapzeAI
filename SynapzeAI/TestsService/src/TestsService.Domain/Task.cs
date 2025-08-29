using System.ComponentModel.DataAnnotations.Schema;
using CSharpFunctionalExtensions;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Id;
using TestsService.Domain.ValueObjects;

namespace TestsService.Domain;

public class Task : Shared.Entity<TaskId>
{
    public int SerialNumber { get; private set; }
    
    public string TaskName { get; private set; }
    public string TaskMessage { get; private set; }
    public string? RightAnswer { get; private set; }
    
    
    private List<TaskStatistic> _taskStatistics = new List<TaskStatistic>();
    public IReadOnlyList<TaskStatistic> TaskStatistics =>  _taskStatistics;
    
    public List<string>? Answers { get; private set; }

    private Task(TaskId id) : base(id)
    {
        
    }
    
    private Task(
        TaskId id,
        string taskName, 
        string taskMessage,
        string? rightAnswer = null,
        IEnumerable<string>? answers = null) : base(id)
    {
        TaskName = taskName;
        TaskMessage = taskMessage;
        RightAnswer = rightAnswer;
        Answers = answers?.ToList();
    }

    public static Result<Task, Error> Create(
        TaskId id,
        string taskName, 
        string taskMessage,
        string? rightAnswer = null,
        IEnumerable<string>? answers = null)
    {
        if (string.IsNullOrWhiteSpace(taskName))
            return Errors.General.ValueIsRequired("название задачи");
        
        if  (string.IsNullOrWhiteSpace(taskMessage))
            return Errors.General.ValueIsRequired("сообщение задачи");
        
        return new Task(id, taskName, taskMessage, rightAnswer, answers);
    }

    internal UnitResult<Error> UpdateInfo(
        string taskName,
        string taskMessage,
        string? rightAnswer = null,
        IEnumerable<string>? answers = null)
    {
        if (string.IsNullOrWhiteSpace(taskName))
            return Errors.General.ValueIsRequired("название задачи");
        
        if  (string.IsNullOrWhiteSpace(taskMessage))
            return Errors.General.ValueIsRequired("сообщение задачи");
        
        TaskName = taskName;
        TaskMessage = taskMessage;
        RightAnswer = rightAnswer;
        Answers = answers?.ToList();

        return Result.Success<Error>();
    }
    
    public void AddTaskStatistic(TaskStatistic statistic) =>
        _taskStatistics.Add(statistic);

    internal UnitResult<Error> SetSerialNumber(int serialNumber)
    {
        if (serialNumber < 1)
            return Errors.General.ValueIsInvalid("серийный номер задачи");
        
        SerialNumber = serialNumber;
        
        return Result.Success<Error>();
    }
}