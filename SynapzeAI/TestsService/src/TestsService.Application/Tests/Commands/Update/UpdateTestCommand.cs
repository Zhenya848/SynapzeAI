using TestsService.Application.Models.Dtos;

namespace TestsService.Application.Tests.Commands.Update;

public record UpdateTestCommand(
    Guid UserId,
    Guid TestId,
    string UniqueUserName,
    string TestName,
    string Theme,
    bool IsPublished,
    int? Seconds,
    int? Minutes,
    IEnumerable<CreateTaskDto>? TasksToCreate,
    IEnumerable<UpdateTaskDto>? TasksToUpdate,
    IEnumerable<Guid>? TaskIdsToDelete);