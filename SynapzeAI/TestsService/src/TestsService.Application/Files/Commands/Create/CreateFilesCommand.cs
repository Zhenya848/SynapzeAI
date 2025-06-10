using TestsService.Domain.Shared.ValueObjects;

namespace TestsService.Application.Files.Commands.Create;

public record CreateFilesCommand(
    IEnumerable<FileData> Files, 
    string BucketName);