using System.ComponentModel.DataAnnotations.Schema;
using CSharpFunctionalExtensions;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Id;
using TestsService.Domain.ValueObjects;

namespace TestsService.Domain;

public class Task : SoftDeletableEntity<TaskId>
{
    public string TaskName { get; private set; }
    public string TaskMessage { get; private set; }
    public string RightAnswer { get; private set; }
    
    public string? ImagePath { get; private set; }
    public string? AudioPath { get; private set; }
    
    public PriorityNumber PriorityNumber { get; private set; }
    
    public TaskStatistic? TaskStatistic { get; private set; }
    
    public List<string>? Answers { get; init; }

    protected Task(TaskId id) : base(id)
    {
        
    }
    
    protected Task(
        TaskId id, 
        string taskName, 
        string taskMessage, 
        string rightAnswer,
        PriorityNumber priorityNumber,
        string? imagePath = null,
        string? audioPath = null,
        IEnumerable<string>? answers = null) : base(id)
    {
        TaskName = taskName;
        TaskMessage = taskMessage;
        RightAnswer = rightAnswer;
        PriorityNumber = priorityNumber;
        ImagePath = imagePath;
        AudioPath = audioPath;
        Answers = answers?.ToList();
    }

    public static Result<Task, Error> Create(
        TaskId id, 
        string taskName, 
        string taskMessage, 
        string rightAnswer,
        PriorityNumber priorityNumber,
        IEnumerable<string>? answers = null,
        string? imagePath = null,
        string? audioPath = null)
    {
        if (string.IsNullOrWhiteSpace(taskName))
            return Errors.General.ValueIsRequired(nameof(taskName));
        
        if  (string.IsNullOrWhiteSpace(taskMessage))
            return Errors.General.ValueIsRequired(nameof(taskMessage));
        
        if (string.IsNullOrWhiteSpace(rightAnswer))
            return Errors.General.ValueIsRequired(nameof(rightAnswer));
        
        return new Task(id, taskName, taskMessage, rightAnswer, priorityNumber, imagePath, audioPath, answers);
    }

    public void UpdateImagePath(string imagePath) =>
        ImagePath = string.IsNullOrWhiteSpace(imagePath) ? ImagePath : imagePath;
    
    public void UpdateAudioPath(string audioPath) =>
        AudioPath = string.IsNullOrWhiteSpace(audioPath) ? AudioPath : audioPath;
    
    public void UpdateStatistic(TaskStatistic statistic) =>
        TaskStatistic = statistic;
    
    public override void Delete()
    {
        base.Delete();
    }

    public override void Restore()
    {
        base.Restore();
    }
}