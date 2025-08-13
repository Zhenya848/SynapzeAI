namespace TestsService.Application.SolvingHistories.Commands.ExplainSolvingAITest;

public record ExplainSolvingTestCommand(
    Guid TestId,
    Guid SolvingHistoryId);