namespace TestsService.Domain.Shared.ValueObjects.Dtos;

public record LimitTimeDto
{
    public int Seconds { get; set; }
    public int Minutes { get; set; }
    public int Hours { get; set; }
}