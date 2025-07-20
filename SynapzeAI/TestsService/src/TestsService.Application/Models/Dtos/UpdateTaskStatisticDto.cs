using TestsService.Domain.Shared.ValueObjects.Dtos;

namespace TestsService.Application.Models.Dtos;

public record UpdateTaskStatisticDto(
    Guid TaskId,
    TaskStatisticDto Statistic);