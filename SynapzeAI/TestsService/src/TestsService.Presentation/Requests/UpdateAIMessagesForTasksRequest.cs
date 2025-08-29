using TestsService.Application.Models.Dtos;

namespace TestsService.Presentation.Requests;

public record UpdateAIMessagesForTasksRequest(IEnumerable<AIMessageForTask> AIMessagesForTasks);