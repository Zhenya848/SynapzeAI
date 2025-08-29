using TestsService.Domain.Shared.ValueObjects.Dtos;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

namespace TestsService.Presentation.Requests;

public record AddSolvingHistoryRequest(
    TaskHistoryDto[] TaskHistories, 
    DateTime SolvingDate,
    int SolvingTimeSeconds);