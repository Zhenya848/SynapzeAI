namespace TestsService.Application.Models.Dtos;

public record CreateTaskDto(
    string TaskName,
    string TaskMessage,
    string? RightAnswer = null,
    string[]? Answers = null);