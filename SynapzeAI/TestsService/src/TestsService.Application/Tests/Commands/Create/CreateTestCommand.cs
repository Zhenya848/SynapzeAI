using TestsService.Application.Models.Dtos;

namespace TestsService.Application.Tests.Commands.Create;

public record CreateTestCommand(
    Guid UserId,
    string UniqueUserName,
    string TestName,
    string Theme,
    bool IsPublished,
    int? Seconds,
    int? Minutes,
    IEnumerable<CreateTaskDto>? Tasks);