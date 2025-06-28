using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestsService.Application.Tasks.Commands.Create;
using TestsService.Application.Tasks.Commands.Delete;
using TestsService.Application.Tasks.Commands.UploadPhotos;
using TestsService.Application.Tests.Commands.Create;
using TestsService.Application.Tests.Commands.Delete;
using TestsService.Application.Tests.Commands.Get;
using TestsService.Application.Tests.Commands.Update;
using TestsService.Application.Tests.Queries;
using TestsService.Domain.Shared.ValueObjects.Dtos;
using TestsService.Presentation.Authorization;
using TestsService.Presentation.Requests;

namespace TestsService.Presentation;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpPost("{userId:guid}/test")]
    [Authorize]
    //[Permission("test.create")]
    public async Task<IActionResult> CreateTest(
        [FromRoute] Guid userId,
        [FromServices] CreateTestHandler handler,
        [FromBody] CreateTestRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateTestCommand(
            userId,
            request.TestName,
            request.IsPublished,
            request.Seconds,
            request.Minutes,
            request.Hours);
        
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPut("{testId:guid}/test")]
    //[Permission("test.update")]
    public async Task<IActionResult> UpdateTest(
        [FromRoute] Guid testId,
        [FromServices] UpdateTestHandler handler,
        [FromBody] UpdateTestRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateTestCommand(
            testId,
            request.TestName,
            request.IsPublished,
            request.Seconds,
            request.Minutes,
            request.Hours);
        
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(Envelope.Ok(result.Value));
    }

    [HttpDelete("{testId:guid}/test")]
    //[Permission("test.delete")]
    public async Task<IActionResult> DeleteTest(
        [FromRoute] Guid testId,
        [FromServices] DeleteTestHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(testId, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPost("{testId:guid}/task")]
    //[Permission("tasks.create")]
    public async Task<IActionResult> CreateTask(
        [FromRoute] Guid testId,
        [FromServices] CreateTasksHandler handler,
        [FromBody] CreateTasksRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateTasksCommand(testId, request.Tasks);
        
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
    
    [HttpDelete("{testId:guid}/task")]
    //[Permission("tasks.delete")]
    public async Task<IActionResult> DeleteTasks(
        [FromRoute] Guid testId,
        [FromForm] IEnumerable<Guid> taskIds,
        [FromServices] DeleteTasksHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteTasksCommand(testId, taskIds);
        
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(Envelope.Ok(result.Value));
    }
    
    [HttpGet("{userId:guid}")]
    //[Permission("test.get")]
    public async Task<ActionResult> GetTestsById(
        [FromRoute] Guid userId,
        [FromServices] GetTestsHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(userId, cancellationToken);
        
        return Ok(Envelope.Ok(result));
    }

    [HttpGet("tests")]
    //[Permission("tests.get")]
    public async Task<ActionResult> GetTestsByPagination(
        [FromQuery] GetTestsWithPaginationRequest request,
        [FromServices] GetTestsWithPaginationHandler handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTestsWithPaginationQuery(request.Page, request.PageSize);
        
        var result = await handler.Handle(query, cancellationToken);
        
        return Ok(Envelope.Ok(result));
    }
}