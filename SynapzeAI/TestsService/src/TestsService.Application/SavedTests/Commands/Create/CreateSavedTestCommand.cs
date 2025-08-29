namespace TestsService.Application.SavedTests.Commands.Create;

public record CreateSavedTestCommand(Guid UserId, Guid TestId);