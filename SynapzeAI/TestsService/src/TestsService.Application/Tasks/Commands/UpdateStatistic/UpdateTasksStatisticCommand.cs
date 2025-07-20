using TestsService.Application.Models.Dtos;

namespace TestsService.Application.Tasks.Commands.UpdateStatistic;

public record UpdateTasksStatisticCommand(
    Guid TestId, 
    IEnumerable<UpdateTaskStatisticDto> Tasks);