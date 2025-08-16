using TestsService.Domain.Shared.ValueObjects.Dtos;

namespace TestsService.Application.SolvingHistories.Commands.Create;

public record AddSolvingHistoryCommand(
    Guid TestId,
    string UniqueUserName,
    string UserEmail,
    TaskHistoryDto[] TaskHistories, 
    DateTime SolvingDate,
    int SolvingTimeSeconds);