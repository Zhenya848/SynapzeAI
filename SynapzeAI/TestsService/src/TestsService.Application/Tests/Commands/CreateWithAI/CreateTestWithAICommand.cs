using TestsService.Domain.Shared.ValueObjects.Dtos;

namespace TestsService.Application.Tests.Commands.CreateWithAI;

public record CreateTestWithAICommand(
    Guid UserId,
    string UniqueUserName,
    string Theme, 
    bool IsTimeLimited, 
    int PercentOfOpenTasks,
    int? TasksCount, 
    int? Difficulty,
    int? Seconds,
    int? Minutes);