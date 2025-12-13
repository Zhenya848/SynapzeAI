namespace TestsService.Domain.Shared.ValueObjects.Infos;

public record TaskInfo
{
    public string TaskName { get; set; }
    public string TaskMessage { get; set; }
    public string RightAnswer { get; set; }
    public string[] Answers {  get; set; }
}