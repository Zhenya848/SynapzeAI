namespace TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

public record TaskDto
{
    public Guid Id { get; set; }
    public Guid TestId { get; set; }
    
    public int SerialNumber { get; set; }
    public string TaskName { get; set; }
    public string TaskMessage { get; set; }
    public string? RightAnswer { get; set; }
    
    public string? ImagePath { get; set; }
    public string? AudioPath { get; set; }
    
    public TaskStatisticDto? TaskStatistic { get; set; }
    
    public string[]?  Answers { get; set; }
}