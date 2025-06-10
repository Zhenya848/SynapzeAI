namespace TestsService.Application.Tests.Commands.Create;

public record CreateTestCommand(
    Guid UserId,
    string TestName,
    bool IsPublished,
    int? Seconds,
    int? Minutes,
    int? Hours);