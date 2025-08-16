using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestsService.Application.Models.Dtos;
using TestsService.Application.SolvingHistories.Commands.Create;
using TestsService.Application.SolvingHistories.Commands.ExplainSolvingAITest;
using TestsService.Application.SolvingHistories.Queries;
using TestsService.Application.Tasks.Commands.UpdateStatistic;
using TestsService.Application.Tasks.Commands.UploadPhotos;
using TestsService.Application.Tests.Commands.Create;
using TestsService.Application.Tests.Commands.CreateWithAI;
using TestsService.Application.Tests.Commands.Delete;
using TestsService.Application.Tests.Commands.Get;
using TestsService.Application.Tests.Commands.GetTest;
using TestsService.Application.Tests.Commands.Update;
using TestsService.Application.Tests.Queries;
using TestsService.Domain.Shared.ValueObjects.Dtos;
using TestsService.Domain.ValueObjects;
using TestsService.Presentation.Authorization;
using TestsService.Presentation.Requests;

namespace TestsService.Presentation;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpPost("{userId:guid}")]
    //[Permission("test.create")]
    public async Task<IActionResult> CreateTest(
        [FromRoute] Guid userId,
        [FromServices] CreateTestHandler handler,
        [FromBody] CreateTestRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateTestCommand(
            userId,
            request.UniqueUserName,
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

    [HttpPost("{userId:guid}/withAI")]
    public async Task<IActionResult> CreateTestWithAI(
        [FromRoute] Guid userId,
        [FromServices] CreateTestWithAIHandler handler,
        [FromBody] CreateTestWithAIRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateTestWithAICommand(
            userId, 
            request.UniqueUserName,
            request.Theme, 
            request.IsTimeLimited,
            request.PercentOfOpenTasks, 
            request.TasksCount, 
            request.Difficulty,
            request.Seconds,
            request.Minutes);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPut("{userId:guid}/{testId:guid}")]
    //[Permission("test.update")]
    public async Task<IActionResult> UpdateTest(
        [FromRoute] Guid userId,
        [FromRoute] Guid testId,
        [FromServices] UpdateTestHandler handler,
        [FromBody] UpdateTestRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateTestCommand(
            userId,
            testId,
            request.UniqueUserName,
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

    [HttpDelete("{userId:guid}/{testId:guid}")]
    //[Permission("test.delete")]
    public async Task<IActionResult> DeleteTest(
        [FromRoute] Guid testId,
        [FromRoute] Guid userId,
        [FromServices] DeleteTestHandler handler,
        CancellationToken cancellationToken = default)
    {
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
        var command = new AddSolvingHistoryCommand(
            testId, 
            request.UniqueUserName,
            request.UserEmail,
            request.TaskHistories, 
            request.SolvingDate,
            request.SolvingTimeSeconds);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(Envelope.Ok(result.Value));
    }

    [HttpPost("{testId:guid}/history")]
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
            request.SearchUserEmail);
        
        var result = await handler.Handle(command, cancellationToken);

        return Ok(Envelope.Ok(result));
    }

    [HttpPut("{testId:guid}/history/explain/{solvingHistoryId:guid}")]
    public async Task<IActionResult> ExplainSolvingTest(
        [FromRoute] Guid testId,
        [FromRoute] Guid solvingHistoryId,
        [FromServices] ExplainSolvingTestHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new ExplainSolvingTestCommand(testId, solvingHistoryId);
        
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPost("{testId:guid}/task/photos")]
    //[Permission("tasks.upload_photos")]
    public async Task<IActionResult> UploadPhotosToTasks(
        [FromRoute] Guid testId,
        [FromForm] IEnumerable<Guid> taskIds,
        [FromForm] IFormFileCollection files,
        [FromServices] UploadFilesToTasksHandler handler,
        CancellationToken cancellationToken = default)
    {
        await using FormFileProcessor formFileProcessor = new FormFileProcessor();
        List<UploadFileDto> fileDtos = formFileProcessor.StartProcess(files);
        
        var command = new UploadFilesToTasksCommand(testId, taskIds, fileDtos);
        
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(Envelope.Ok(result.Value));
    }

    [HttpPut("{testId:guid}/task")]
    public async Task<IActionResult> UpdateTasksStatistic(
        [FromRoute] Guid testId,
        [FromBody] UpdateTasksStatisticRequest request,
        [FromServices] UpdateTasksStatisticHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateTasksStatisticCommand(testId, request.Tasks);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(Envelope.Ok(null));
    }
    
    [HttpGet("{userId:guid}/tests")]
    [Permission("test.create")] 
    public async Task<ActionResult> GetTestsById(
        [FromRoute] Guid userId,
        [FromServices] GetTestsHandler handler,
        CancellationToken cancellationToken = default)
    {
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
    //[Permission("tests.get")]
    public async Task<ActionResult> GetTestsByPagination(
        [FromBody] GetTestsWithPaginationRequest request,
        [FromServices] GetTestsWithPaginationHandler handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTestsWithPaginationQuery(
            request.Page, 
            request.PageSize,
            request.SearchTestName,
            request.SearchTestTheme,
            request.SearchUserName,
            request.OrderBy);
        
        var result = await handler.Handle(query, cancellationToken);
        
        return Ok(Envelope.Ok(result));
    }
}