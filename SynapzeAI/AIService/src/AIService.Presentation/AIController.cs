using AIService.Application.Commands.Generate;
using AIService.Domain;
using AIService.Presentation.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AIService.Presentation;

[ApiController]
[Route("api/[controller]")]
public class AIController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> GenerateContent(
        [FromBody] GenerateContentRequest request,
        [FromServices] GenerateContentHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new GenerateContentCommand(request.UserRequest, AIModel.Deepseek);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}