using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

namespace TestsService.Domain.ValueObjects;

public record TaskHistory
{
    public int SerialNumber { get; init; }
    public string TaskName { get; init; }
    public string TaskMessage { get; init; }
    public string? RightAnswer { get; init; }
    
    public string? ImagePath { get; init; }
    public string? AudioPath { get; init; }
    
    public List<string>?  Answers { get; init; }
    public string UserAnswer { get; init; }

    public string? MessageAI { get; private set; }
    
    [JsonConstructor]
    private TaskHistory()
    {
        
    }
    
    private TaskHistory(
        int serialNumber,
        string taskName, 
        string taskMessage, 
        string userAnswer, 
        string? rightAnswer = null,
        List<string>? answers = null,
        string? messageAI = null,
        string? imagePath = null,
        string? audioPath = null)
    {
        SerialNumber = serialNumber;
        TaskName = taskName;
        TaskMessage = taskMessage;
        UserAnswer = userAnswer;
        RightAnswer = rightAnswer;
        Answers = answers;
        MessageAI = messageAI;
        ImagePath = imagePath;
        AudioPath = audioPath;
    }

    public static Result<TaskHistory, Error> Create(
        int serialNumber,
        string taskName, 
        string taskMessage, 
        string userAnswer, 
        string? rightAnswer = null,
        IEnumerable<string>? answers = null,
        string? messageAI = null,
        string? imagePath = null,
        string? audioPath = null)
    {
        if (serialNumber < 1)
            return Errors.General.ValueIsInvalid(nameof(serialNumber));
        
        if (string.IsNullOrWhiteSpace(taskName))
            return Errors.General.ValueIsRequired(nameof(taskName));
        
        if  (string.IsNullOrWhiteSpace(taskMessage))
            return Errors.General.ValueIsRequired(nameof(taskMessage));
        
        if (string.IsNullOrWhiteSpace(userAnswer))
            return Errors.General.ValueIsRequired(nameof(userAnswer));
        
        return new TaskHistory(
            serialNumber,
            taskName,
            taskMessage,
            userAnswer,
            rightAnswer,
            answers?.ToList(),
            messageAI,
            imagePath,
            audioPath);
    }
    
    public UnitResult<Error> UpdateAIMessage(string? aiMessage)
    {
        if (string.IsNullOrWhiteSpace(aiMessage))
            return Errors.General.ValueIsInvalid(nameof(aiMessage));
        
        MessageAI = aiMessage;

        return Result.Success<Error>();
    }
}