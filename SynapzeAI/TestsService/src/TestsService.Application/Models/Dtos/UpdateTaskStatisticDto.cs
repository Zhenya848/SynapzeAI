using TestsService.Domain.Shared.ValueObjects.Dtos;

namespace TestsService.Application.Models.Dtos;

public record UpdateTaskStatisticDto(
    Guid TaskId,
    int ErrorsCount,
    int RightAnswersCount,
    DateTime LastReviewTime,
    float AvgTimeSolvingSec);