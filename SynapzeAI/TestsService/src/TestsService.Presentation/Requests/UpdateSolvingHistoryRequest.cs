using TestsService.Application.Models.Dtos;

namespace TestsService.Presentation.Requests;

public record UpdateSolvingHistoryRequest(IEnumerable<UpdateTaskHistoryDto> Tasks);