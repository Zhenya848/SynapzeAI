namespace TestsService.Domain.Shared.ValueObjects.Dtos;

public record TaskStatisticDto
{
    public int ErrorsCount { get; set; }
    public int RightAnswersCount { get; set; }
}