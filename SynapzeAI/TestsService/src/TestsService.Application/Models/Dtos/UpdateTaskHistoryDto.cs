namespace TestsService.Application.Models.Dtos;

public record UpdateTaskHistoryDto(
    int SerialNumber,
    string Message,
    int Points);