namespace TestsService.Domain.Shared.ValueObjects.Infos;

public record TaskHistoryInfo
{
    public int SerialNumber { get; set; }
    
    public string TaskName { get; set; }
    public string TaskMessage { get; set; }
    public string? RightAnswer { get; set; }
    
    public string[]?  Answers { get; set; }
    public string UserAnswer { get; set; }
}