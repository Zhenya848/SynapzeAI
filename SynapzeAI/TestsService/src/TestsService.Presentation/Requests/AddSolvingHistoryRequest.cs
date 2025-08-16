using TestsService.Domain.Shared.ValueObjects.Dtos;

namespace TestsService.Presentation.Requests;

public record AddSolvingHistoryRequest(
    string UniqueUserName,
    string UserEmail,
    TaskHistoryDto[] TaskHistories, 
    DateTime SolvingDate,
    int SolvingTimeSeconds);