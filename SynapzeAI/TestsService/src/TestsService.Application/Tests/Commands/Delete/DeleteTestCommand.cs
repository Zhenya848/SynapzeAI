namespace TestsService.Application.Tests.Commands.Delete;

public record DeleteTestCommand(
    Guid UserId, 
    Guid TestId);