namespace TestsService.Application.SavedTests.Commands.Delete;

public record DeleteSavedTestCommand(Guid UserId, Guid TestId);