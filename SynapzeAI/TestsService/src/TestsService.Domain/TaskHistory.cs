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

    public string? MessageAI { get; private set; }

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
        string? messageAI) : base(id)
    {
        SerialNumber = serialNumber;
        TaskName = taskName;
        TaskMessage = taskMessage;
        RightAnswer = rightAnswer;
        Answers = answers;
        UserAnswer = userAnswer;
        MessageAI = messageAI;
    }
    
    public static Result<TaskHistory, Error> Create(
        int serialNumber,
        string taskName, 
        string taskMessage, 
        string userAnswer, 
        string? rightAnswer = null,
        IEnumerable<string>? answers = null,
        string? messageAI = null)
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
            answers?.ToList(),
            messageAI);
    }
    
    public UnitResult<Error> UpdateAIMessage(string? aiMessage)
    {
        if (string.IsNullOrWhiteSpace(aiMessage))
            return Errors.General.ValueIsInvalid("сообщение от AI");
        
        MessageAI = aiMessage;

        return Result.Success<Error>();
    }
}