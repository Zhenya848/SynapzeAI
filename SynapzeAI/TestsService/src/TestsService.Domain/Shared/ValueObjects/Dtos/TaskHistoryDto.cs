namespace TestsService.Domain.Shared.ValueObjects.Dtos;

public record TaskHistoryDto
{
    public string TaskName { get; set; }
    public string TaskMessage { get; set; }
    public string? RightAnswer { get; set; }
    
    public string? ImagePath { get; set; }
    public string? AudioPath { get; set; }
    
    public string[]?  Answers { get; set; }
    public string UserAnswer { get; set; }

    public string? MessageAI { get; set; }
}