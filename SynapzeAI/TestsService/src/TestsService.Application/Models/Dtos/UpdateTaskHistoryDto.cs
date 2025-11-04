namespace TestsService.Application.Models.Dtos;

public record UpdateTaskHistoryDto(
    Guid TaskHistoryId,
    string Message,
    int Points);