using TestsService.Domain.Shared.ValueObjects.Dtos;

namespace TestsService.Presentation.Requests;

public record CreateTestWithAIRequest(
    string Theme, 
    bool IsTimeLimited, 
    int PercentOfOpenTasks,
    int? TasksCount, 
    int? Difficulty,
    int? Seconds,
    int? Minutes);