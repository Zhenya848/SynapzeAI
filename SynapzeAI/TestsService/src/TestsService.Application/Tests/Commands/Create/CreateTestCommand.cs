namespace TestsService.Application.Tests.Commands.Create;

public record CreateTestCommand(
    Guid UserId,
    string TestName,
    string Theme,
    bool IsPublished,
    int? Seconds,
    int? Minutes);