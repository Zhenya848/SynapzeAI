namespace TestsService.Application.Models.Dtos;

public record UpdateTaskDto(
    Guid TaskId,
    string TaskName,
    string TaskMessage,
    string? RightAnswer = null,
    string[]? Answers = null);