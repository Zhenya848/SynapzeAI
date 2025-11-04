using Framework;
using Framework.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestsService.Application.SavedTests.Commands.Create;
using TestsService.Application.SavedTests.Commands.Delete;
using TestsService.Application.SavedTests.Queries.Get;
using TestsService.Application.SolvingHistories.Commands.Create;
using TestsService.Application.SolvingHistories.Commands.Update;
using TestsService.Application.SolvingHistories.Querise;
using TestsService.Application.TaskStatistics.Commands.Update;
using TestsService.Application.Tests.Commands.Create;
using TestsService.Application.Tests.Commands.Delete;
using TestsService.Application.Tests.Commands.GetTest;
using TestsService.Application.Tests.Commands.GetTests;
using TestsService.Application.Tests.Commands.Update;
using TestsService.Application.Tests.Queries;
using TestsService.Presentation.Requests;

namespace TestsService.Presentation;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateTest(
        [FromServices] CreateTestHandler handler,
        [FromBody] CreateTestRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserIdRequired();
        var userName = User.GetUserNameRequired();
        
        var command = new CreateTestCommand(
            userId,
            userName,
            request.TestName,
            request.Theme,
            request.IsPublished,
            request.Seconds,
            request.Minutes,
            request.Tasks);
        
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPut("{testId:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateTest(
        [FromRoute] Guid testId,
        [FromServices] UpdateTestHandler handler,
        [FromBody] UpdateTestRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserIdRequired();
        var userName = User.GetUserNameRequired();
        
        var command = new UpdateTestCommand(
            userId,
            testId,
            userName,
            request.TestName,
            request.Theme,
            request.IsPublished,
            request.Seconds,
            request.Minutes,
            request.TasksToCreate,
            request.TasksToUpdate,
            request.TaskIdsToDelete);
        
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(Envelope.Ok(result.Value));
    }

    [HttpDelete("{testId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteTest(
        [FromRoute] Guid testId,
        [FromServices] DeleteTestHandler handler,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserIdRequired();
        
        var command = new DeleteTestCommand(userId, testId);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(Envelope.Ok(result.Value));
    }

    [HttpPut("{testId:guid}/history")]
    public async Task<IActionResult> AddSolvingHistory(
        [FromRoute] Guid testId,
        [FromBody] AddSolvingHistoryRequest request,
        [FromServices] AddSolvingHistoryHandler handler,
        CancellationToken cancellationToken = default)
    {
        var userName = User.GetUserName() ?? "none";
        var userTelegram = User.GetUserTelegram() ?? "none";
        
        var command = new AddSolvingHistoryCommand(
            testId,
            userName,
            userTelegram,
            request.TaskHistories, 
            request.SolvingDate,
            request.SolvingTimeSeconds);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(Envelope.Ok(result.Value));
    }

    [HttpPost("{testId:guid}/history")]
    [Authorize]
    public async Task<IActionResult> GetSolvingHistoriesByPagination(
        [FromRoute] Guid testId,
        [FromServices] GetSolvingHistoriesByPaginationHandler handler,
        [FromBody] GetSolvingHistoriesByPaginationRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new GetSolvingHistoriesByPaginationQuery(
            request.Page,
            request.PageSize,
            testId,
            request.SearchUserName,
            request.SearchUserTelegram,
            request.OrderBy);
        
        var result = await handler.Handle(command, cancellationToken);

        return Ok(Envelope.Ok(result));
    }

    [HttpPut("history/update/{solvingHistoryId:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateSolvingHostory(
        [FromRoute] Guid solvingHistoryId,
        [FromBody] UpdateSolvingHistoryRequest request,
        [FromServices] UpdateSolvingHistoryHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateSolvingHistoryCommand(
            solvingHistoryId, request.Tasks);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }

    [HttpPut("tasksStatistic")]
    [Authorize]
    public async Task<IActionResult> UpdateTasksStatistics(
        [FromBody] UpdateTasksStatisticsRequest request,
        [FromServices] UpdateTasksStatisticsHandler handler,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserIdRequired();
        
        var command = new UpdateTasksStatisticsCommand(userId, request.Tasks);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }
    
    [HttpGet("tests")]
    [Authorize]
    public async Task<ActionResult> GetTestsById(
        [FromServices] GetTestsHandler handler,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserIdRequired();
        
        var result = await handler.Handle(userId, cancellationToken);
        
        return Ok(Envelope.Ok(result));
    }
    
    [HttpGet("{testId:guid}/test")]
    public async Task<ActionResult> GetTestById(
        [FromRoute] Guid testId,
        [FromServices] GetTestHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(testId, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(Envelope.Ok(result.Value));
    }

    [HttpPost("tests")]
    public async Task<ActionResult> GetTestsByPagination(
        [FromBody] GetTestsWithPaginationRequest request,
        [FromServices] GetTestsWithPaginationHandler handler,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        
        var query = new GetTestsWithPaginationQuery(
            request.Page, 
            request.PageSize,
            request.SearchTestName,
            request.SearchTestTheme,
            request.SearchUserName,
            request.OrderBy,
            userId);
        
        var result = await handler.Handle(query, cancellationToken);
        
        return Ok(Envelope.Ok(result));
    }

    [HttpPost("{testId:guid}/saved")]
    [Authorize]
    public async Task<IActionResult> CreateSavedTest(
        [FromRoute] Guid testId,
        [FromServices] CreateSavedTestHandler handler,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserIdRequired();
        var command = new CreateSavedTestCommand(userId, testId);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(Envelope.Ok(result.Value));
    }

    [HttpPost("tests/saved")]
    [Authorize]
    public async Task<IActionResult> GetSavedTestsByPagination(
        [FromBody] GetSavedTestsWithPaginationRequest request,
        [FromServices] GetSavedTestsWithPaginationHandler handler,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserIdRequired();

        var command = new GetSavedTestsWithPaginationQuery(
            userId,
            request.Page,
            request.PageSize,
            request.SearchTestName,
            request.SearchTestTheme,
            request.SearchUserName,
            request.OrderBy);
        
        var result = await handler.Handle(command, cancellationToken);
        
        return Ok(Envelope.Ok(result));
    }

    [HttpDelete("{testId:guid}/saved")]
    [Authorize]
    public async Task<IActionResult> DeleteSavedTest(
        [FromRoute] Guid testId,
        [FromServices] DeleteSavedTestHandler handler,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserIdRequired();
        var command = new DeleteSavedTestCommand(userId, testId);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(Envelope.Ok(result.Value));
    }
}