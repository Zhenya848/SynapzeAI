using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using TestsService.Application.Files.Commands.Create;
using TestsService.Application.Files.Commands.Delete;
using TestsService.Application.Providers;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects;

namespace TestsService.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private readonly IMinioClient _minioClient;
    private readonly ILogger _logger;
    
    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }
    
    public async Task<Result<IReadOnlyList<string>, ErrorList>> UploadFiles(
        CreateFilesCommand command, 
        CancellationToken cancellationToken = default)
    {
        var semaphoreSlim = new SemaphoreSlim(5);

        try
        {
            var bucketExistArgs = new BucketExistsArgs().WithBucket(command.BucketName);
            var bucketExist = await _minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);

            if (bucketExist == false)
            {
                var makeBucketArgs = new MakeBucketArgs().WithBucket(command.BucketName);
                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }

            var fileTasks = command.Files
                .Select(async f => await PutObject(f, semaphoreSlim, cancellationToken, command.BucketName));
            
            var result = await Task.WhenAll(fileTasks);
            
            if (result.Any(r => r.IsFailure))
                return (ErrorList)result.Select(e => e.Error);

            return command.Files.Select(p => p.FilePath).ToList();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while uploading files. " + e.Message);
            return (ErrorList)Error.Failure("file.upload", "Fail to upload files in minio.");
        }
    }

    public async Task<Result<string, ErrorList>> DeleteFile(
        DeleteFileCommand command, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (await IsFileExist(command.BucketName, command.ObjectName) == false)
                return (ErrorList)Error.NotFound("file.not.found", $"File with name {command.ObjectName} not found!");
        
            var removeFileArgs = new RemoveObjectArgs()
                .WithBucket(command.BucketName)
                .WithObject(command.ObjectName);
        
            await _minioClient.RemoveObjectAsync(removeFileArgs, cancellationToken);
        
            return command.ObjectName;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while deleting files. " + e.Message);
            return (ErrorList)Error.Failure("file.delete", "Fail to delete file in minio");
        }
    }

    public async Task<Result<IReadOnlyList<FileData>, ErrorList>> GetFiles(
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private async Task<UnitResult<Error>> PutObject(
        FileData file,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken,
        string bucketName)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithStreamData(file.Stream)
            .WithObjectSize(file.Stream.Length)
            .WithObject(file.FilePath);

        try
        {
            await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Fail to upload file in minio with path {path} in bucket {bucket}. {exception}",
                file.FilePath,
                bucketName,
                e);
            
            return Error.Failure("file.upload", "Failed to upload file in minio");
        }
        finally { semaphoreSlim.Release(); }
        
        return Result.Success<Error>();
    }
    
    private async Task<bool> IsFileExist(string bucketName, string objectName)
    {
        var fileExistArgs = new StatObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName);
        var fileExist = await _minioClient.StatObjectAsync(fileExistArgs);

        return fileExist.ContentType != null;
    }
}