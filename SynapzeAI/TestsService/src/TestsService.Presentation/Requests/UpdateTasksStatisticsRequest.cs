using TestsService.Application.Models.Dtos;

namespace TestsService.Presentation.Requests;

public record UpdateTasksStatisticsRequest(IEnumerable<UpdateTaskStatisticDto> Tasks);