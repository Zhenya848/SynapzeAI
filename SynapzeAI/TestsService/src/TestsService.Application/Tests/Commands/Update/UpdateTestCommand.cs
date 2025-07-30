using TestsService.Application.Models.Dtos;

namespace TestsService.Application.Tests.Commands.Update;

public record UpdateTestCommand(
    Guid TestId,
    string TestName,
    string Theme,
    bool WithAI,
    int? Seconds,
    int? Minutes,
    IEnumerable<CreateTaskDto>? TasksToCreate,
    IEnumerable<UpdateTaskDto>? TasksToUpdate,
    IEnumerable<Guid>? TaskIdsToDelete);