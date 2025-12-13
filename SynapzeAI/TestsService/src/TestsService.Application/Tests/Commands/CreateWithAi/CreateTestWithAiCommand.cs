namespace TestsService.Application.Tests.Commands.CreateWithAi;

public record CreateTestWithAiCommand(
    Guid UserId,
    string UniqueUserName,
    string TestTheme,
    int PercentOfOpenTasks,
    int Difficulty,
    int? TasksCount = null,
    int? Seconds = null,
    int? Minutes = null,
    string? Base64File = null);