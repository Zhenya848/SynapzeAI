using TestsService.Application.Models.Dtos;

namespace TestsService.Application.TaskStatistics.Commands.Update;

public record UpdateTasksStatisticsCommand(
    Guid UserId,
    IEnumerable<UpdateTaskStatisticDto> TasksStatistics);