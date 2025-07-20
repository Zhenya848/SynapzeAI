using TestsService.Application.Models.Dtos;

namespace TestsService.Presentation.Requests;

public record UpdateTasksStatisticRequest(IEnumerable<UpdateTaskStatisticDto> Tasks);