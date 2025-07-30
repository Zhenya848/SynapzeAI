using TestsService.Application.Models.Dtos;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

namespace TestsService.Application.Tests.Commands.Create;

public record CreateTestCommand(
    Guid UserId,
    string TestName,
    string Theme,
    bool WithAI,
    int? Seconds,
    int? Minutes,
    IEnumerable<CreateTaskDto>? Tasks);