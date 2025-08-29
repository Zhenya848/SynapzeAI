namespace TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

public record TaskStatisticDto
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    public Guid TaskId { get; set; }
    
    public int ErrorsCount { get; set; }
    public int RightAnswersCount { get; set; }
    public DateTime LastReviewTime { get; set; }
    public float AvgTimeSolvingSec { get; set; }
}