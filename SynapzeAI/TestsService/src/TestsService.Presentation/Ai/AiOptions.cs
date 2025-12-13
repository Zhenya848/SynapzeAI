namespace TestsService.Presentation.Ai;

public record AiOptions
{
    public const string Ai = "AiOptions";
    public string ApiKey { get; init; }
    public string Model { get; init; }
}