using CSharpFunctionalExtensions;
using TestsService.Application.Files.Commands.Create;
using TestsService.Application.Files.Commands.Delete;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects;

namespace TestsService.Application.Providers;

public interface IFileProvider
{
    public Task<Result<IReadOnlyList<string>, ErrorList>> UploadFiles(
        CreateFilesCommand command,
        CancellationToken cancellationToken = default);
    
    public Task<Result<string, ErrorList>> DeleteFile(
        DeleteFileCommand command,
        CancellationToken cancellationToken = default);
    
    public Task<Result<IReadOnlyList<FileData>, ErrorList>> GetFiles(CancellationToken cancellationToken = default);
}