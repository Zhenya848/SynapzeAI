namespace TestsService.Application.Tests.Commands.Update;

public record UpdateTestCommand(
    Guid TestId,
    string TestName,
    string Theme,
    bool IsPublished,
    int? Seconds,
    int? Minutes);