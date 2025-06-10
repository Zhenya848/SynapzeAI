using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TestsService.Application.Files.Commands.Delete;
using TestsService.Application.Messaging;
using TestsService.Application.Providers;
using FileInfo = TestsService.Domain.Shared.ValueObjects.FileInfo;

namespace TestsService.Infrastructure.BackgroundServices;

public class FileCleanerBackgroundService : BackgroundService
{
    private readonly ILogger<FileCleanerBackgroundService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;

    public FileCleanerBackgroundService(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<FileCleanerBackgroundService> logger,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _messageQueue = messageQueue;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("FileCleanerBackgroundService is starting.");
        
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var fileProvider = scope.ServiceProvider.GetRequiredService<IFileProvider>();
        
        while (stoppingToken.IsCancellationRequested == false)
        {
            var fileInfos = await _messageQueue.ReadAsync(stoppingToken);

            foreach (var file in fileInfos)
            {
                var command = new DeleteFileCommand(file.BucketName, file.ObjectName);
                
                await fileProvider.DeleteFile(command, stoppingToken);
            }
        }
        
        await Task.Yield();
    }
}