namespace TestsService.Application.Tasks.Commands.Delete;

public record DeleteTasksCommand(
    Guid TestId,
    IEnumerable<Guid> TasIds);