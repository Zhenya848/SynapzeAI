namespace TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

public record SolvingHistoryDto
{
    public Guid Id { get; set; }
    public Guid TestId { get; set; }
    
    public string UniqueUserName { get; set; }
    public string UserEmail { get; set; }
    
    public TaskHistoryDto[] TaskHistories { get; set; }
    public DateTime SolvingDate { get; set; }
    public int SolvingTimeSeconds { get; set; }
}