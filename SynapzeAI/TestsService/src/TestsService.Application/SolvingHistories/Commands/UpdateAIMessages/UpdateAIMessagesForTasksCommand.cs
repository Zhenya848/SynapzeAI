using TestsService.Application.Models.Dtos;

namespace TestsService.Application.SolvingHistories.Commands.UpdateAIMessages;

public record UpdateAIMessagesForTasksCommand(
    Guid SolvingHistoryId,
    IEnumerable<AIMessageForTask> AIMessagesForTasks);