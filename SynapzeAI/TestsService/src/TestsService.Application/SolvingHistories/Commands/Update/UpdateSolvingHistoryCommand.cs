using TestsService.Application.Models.Dtos;

namespace TestsService.Application.SolvingHistories.Commands.Update;

public record UpdateSolvingHistoryCommand(
    Guid SolvingHistoryId,
    IEnumerable<UpdateTaskHistoryDto> Tasks);