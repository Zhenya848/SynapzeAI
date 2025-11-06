namespace TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

public record TaskHistoryDto
{
    public Guid Id { get; set; }
    public Guid SolvingHistoryId { get; set; }
    
    public int SerialNumber { get; set; }
    
    public string TaskName { get; set; }
    public string TaskMessage { get; set; }
    public string? RightAnswer { get; set; }
    
    public string[]?  Answers { get; set; }
    public string UserAnswer { get; set; }

    public string? Message { get; set; }
    public int? Points { get; set; }
}