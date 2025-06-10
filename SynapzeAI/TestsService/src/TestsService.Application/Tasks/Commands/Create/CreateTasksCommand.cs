using TestsService.Application.Models.Dtos;

namespace TestsService.Application.Tasks.Commands.Create;

public record CreateTasksCommand(
    Guid TestId,
    IEnumerable<CreateTaskDto> Tasks);