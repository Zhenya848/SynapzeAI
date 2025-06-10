namespace TestsService.Application.Files.Commands.Delete;

public record DeleteFileCommand(string BucketName, string ObjectName);