using TestsService.Application.Models.Dtos;

namespace TestsService.Presentation.Requests;

public record CreateTasksRequest(IEnumerable<CreateTaskDto> Tasks);