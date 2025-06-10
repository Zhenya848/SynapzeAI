namespace TestsService.Domain.ValueObjects;

public record TaskStatistic
{
    public int ErrorsCount { get; set; }
    public int RightAnswersCount { get; set; }
}