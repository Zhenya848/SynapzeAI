using Core;
using CSharpFunctionalExtensions;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Id;

namespace TestsService.Domain;

public class TaskHistory : Core.Entity<TaskHistoryId>
{
    public int SerialNumber { get; init; }
    
    public string TaskName { get; init; }
    public string TaskMessage { get; init; }
    public string? RightAnswer { get; init; }
    
    public List<string>?  Answers { get; init; }
    public string UserAnswer { get; init; }

    public string? Message { get; private set; }

    public int? Points
    {
        get => Points;
        set
        {
            Points = value < 0 ? 0 : value;
            Points = value > 100 ? 100 : value;
        }
    }

    private TaskHistory(TaskHistoryId id) :  base(id)
    {
        
    }
    
    public TaskHistory(
        TaskHistoryId id, 
        int serialNumber, 
        string taskName, 
        string taskMessage, 
        string userAnswer,
        string? rightAnswer,
        List<string>? answers,  
        string? message,
        int? points) : base(id)
    {
        SerialNumber = serialNumber;
        TaskName = taskName;
        TaskMessage = taskMessage;
        RightAnswer = rightAnswer;
        Answers = answers;
        UserAnswer = userAnswer;
        Message = message;
        Points = points;
    }
    
    public static Result<TaskHistory, Error> Create(
        int serialNumber,
        string taskName, 
        string taskMessage, 
        string userAnswer, 
        string? rightAnswer = null,
        IEnumerable<string>? answers = null,
        string? message = null,
        int? points = null)
    {
        if (serialNumber < 1)
            return Errors.General.ValueIsInvalid("серийный номер задачи");
        
        if (string.IsNullOrWhiteSpace(taskName))
            return Errors.General.ValueIsRequired("название задачи");
        
        if  (string.IsNullOrWhiteSpace(taskMessage))
            return Errors.General.ValueIsRequired("сообщение задачи");
        
        if (string.IsNullOrWhiteSpace(userAnswer))
            return Errors.General.ValueIsRequired("ответ пользователя");
        
        points = points < 0 ? 0 : points;
        points = points > 100 ? 100 : points;
        
        return new TaskHistory(
            TaskHistoryId.AddNewId(),
            serialNumber,
            taskName,
            taskMessage,
            userAnswer,
            rightAnswer,
            answers?.ToList(),
            message,
            points);
    }
    
    public UnitResult<Error> UpdateMessage(string? message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return Errors.General.ValueIsInvalid("сообщение");
        
        Message = message;

        return Result.Success<Error>();
    }
}