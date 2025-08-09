namespace TestsService.Application.SolvingHistories.Commands.ExplainLastSolvingAITest;

public record ExplainSolvingTestCommand(
    Guid TestId,
    Guid SolvingHistoryId);