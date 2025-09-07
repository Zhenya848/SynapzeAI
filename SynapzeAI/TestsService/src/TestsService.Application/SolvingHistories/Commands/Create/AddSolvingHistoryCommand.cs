using TestsService.Domain.Shared.ValueObjects.Dtos;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

namespace TestsService.Application.SolvingHistories.Commands.Create;

public record AddSolvingHistoryCommand(
    Guid TestId,
    string UniqueUserName,
    string UserTelegram,
    TaskHistoryDto[] TaskHistories, 
    DateTime SolvingDate,
    int SolvingTimeSeconds);