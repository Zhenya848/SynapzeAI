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

    public int? Points { get; private set; }

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
        List<string>? answers) : base(id)
    {
        SerialNumber = serialNumber;
        TaskName = taskName;
        TaskMessage = taskMessage;
        RightAnswer = rightAnswer;
        Answers = answers;
        UserAnswer = userAnswer;
    }
    
    public static Result<TaskHistory, Error> Create(
        int serialNumber,
        string taskName, 
        string taskMessage, 
        string userAnswer, 
        string? rightAnswer = null,
        IEnumerable<string>? answers = null)
    {
        if (serialNumber < 1)
            return Errors.General.ValueIsInvalid("серийный номер задачи");
        
        if (string.IsNullOrWhiteSpace(taskName))
            return Errors.General.ValueIsRequired("название задачи");
        
        if  (string.IsNullOrWhiteSpace(taskMessage))
            return Errors.General.ValueIsRequired("сообщение задачи");
        
        if (string.IsNullOrWhiteSpace(userAnswer))
            return Errors.General.ValueIsRequired("ответ пользователя");
        
        return new TaskHistory(
            TaskHistoryId.AddNewId(),
            serialNumber,
            taskName,
            taskMessage,
            userAnswer,
            rightAnswer,
            answers?.ToList());
    }
    
    public UnitResult<Error> UpdateMessage(string? message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return Errors.General.ValueIsInvalid("сообщение");
        
        Message = message;

        return Result.Success<Error>();
    }

    public void UpdatePoints(int? points)
    {
        if (points is not null)
        {
            Points = points < 0 ? 0 : points;
            Points = points > 100 ? 100 : points;
        }
        else
            Points = points;
    }
}