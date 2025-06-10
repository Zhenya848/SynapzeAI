using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using TestsService.Application.Abstractions;
using TestsService.Application.Messaging;
using TestsService.Application.Repositories;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Id;
using FileInfo = TestsService.Domain.Shared.ValueObjects.FileInfo;

namespace TestsService.Application.Tasks.Commands.Delete;

public class DeleteTasksHandler : ICommandHandler<DeleteTasksCommand, Result<IEnumerable<Guid>, ErrorList>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITestRepository _testRepository;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;

    public DeleteTasksHandler(
        IUnitOfWork unitOfWork, 
        ITestRepository testRepository,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue)
    {
        _unitOfWork = unitOfWork;
        _testRepository = testRepository;
        _messageQueue = messageQueue;
    }
    
    public async Task<Result<IEnumerable<Guid>, ErrorList>> Handle(
        DeleteTasksCommand command, 
        CancellationToken cancellationToken = default)
    {
        var testResult = await _testRepository
            .GetById(TestId.Create(command.TestId), cancellationToken);
        
        if (testResult.IsFailure)
            return testResult.Error;
        
        var test = testResult.Value;
        
        var tasks = test
            .GetTasksByIds(command.TasIds);

        var files = tasks
            .Select(t => new FileInfo("photos", t.ImagePath));
        
        await _messageQueue.WriteAsync(files, cancellationToken);
        _testRepository.DeleteTasks(tasks);
        
        await _unitOfWork.SaveChanges(cancellationToken);

        return Result.Success<IEnumerable<Guid>, ErrorList>(tasks.Select(i => i.Id.Value));
    }
}