using TestsService.Domain.Shared.ValueObjects.Dtos;

namespace TestsService.Presentation.Requests;

public record AddSolvingHistoryRequest(
    Guid UserId,
    TaskHistoryDto[] TaskHistories, 
    DateTime SolvingDate,
    int SolvingTimeSeconds);