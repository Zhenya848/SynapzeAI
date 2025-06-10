namespace TestsService.Application.Tests.Commands.Update;

public record UpdateTestCommand(
    Guid TestId,
    string TestName,
    bool IsPublished,
    int? Seconds,
    int? Minutes,
    int? Hours);