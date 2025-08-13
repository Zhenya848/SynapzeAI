using AIService.Domain;

namespace AIService.Application.Commands.Generate;

public record GenerateContentCommand(string UserRequest, AIModel AIModel);