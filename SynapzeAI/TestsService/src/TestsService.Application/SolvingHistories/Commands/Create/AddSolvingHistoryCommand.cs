using TestsService.Domain.Shared.ValueObjects.Dtos;

namespace TestsService.Application.SolvingHistories.Commands.Create;

public record AddSolvingHistoryCommand(
    Guid UserId,
    Guid TestId, 
    TaskHistoryDto[] TaskHistories, 
    DateTime SolvingDate,
    int SolvingTimeSeconds);