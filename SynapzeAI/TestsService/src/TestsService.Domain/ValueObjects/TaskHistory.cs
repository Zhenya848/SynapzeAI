using CSharpFunctionalExtensions;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

namespace TestsService.Domain.ValueObjects;

public record TaskHistory
{
    public string TaskName { get; set; }
    public string TaskMessage { get; set; }
    public string? RightAnswer { get; set; }
    
    public string? ImagePath { get; set; }
    public string? AudioPath { get; set; }
    
    public List<string>?  Answers { get; set; }
    public string UserAnswer { get; }

    public string? MessageAI { get; }

    private TaskHistory(
        string taskName, 
        string taskMessage, 
        string userAnswer, 
        string? rightAnswer = null,
        List<string>? answers = null,
        string? messageAI = null,
        string? imagePath = null,
        string? audioPath = null)
    {
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
        string taskName, 
        string taskMessage, 
        string userAnswer, 
        string? rightAnswer = null,
        IEnumerable<string>? answers = null,
        string? messageAI = null,
        string? imagePath = null,
        string? audioPath = null)
    {
        if (string.IsNullOrWhiteSpace(taskName))
            return Errors.General.ValueIsRequired(nameof(taskName));
        
        if  (string.IsNullOrWhiteSpace(taskMessage))
            return Errors.General.ValueIsRequired(nameof(taskMessage));
        
        if (string.IsNullOrWhiteSpace(userAnswer))
            return Errors.General.ValueIsRequired(nameof(userAnswer));
        
        return new TaskHistory(
            taskName,
            taskMessage,
            userAnswer,
            rightAnswer,
            answers?.ToList(),
            messageAI,
            imagePath,
            audioPath);
    }
}