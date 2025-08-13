using CSharpFunctionalExtensions;
using TestsService.Application.Abstractions;
using TestsService.Application.Messaging;
using TestsService.Application.Repositories;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Id;
using TestsService.Domain.ValueObjects;
using Task = TestsService.Domain.Task;
using FileInfo = TestsService.Domain.Shared.ValueObjects.FileInfo;

namespace TestsService.Application.Tests.Commands.Update;

public class UpdateTestHandler : ICommandHandler<UpdateTestCommand, Result<Guid, ErrorList>>
{
    private readonly ITestRepository _testRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
    public UpdateTestHandler(
        IUnitOfWork unitOfWork,
        ITestRepository testRepository,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue)
    {
        _testRepository = testRepository;
        _unitOfWork = unitOfWork;
        _messageQueue = messageQueue;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateTestCommand command, 
        CancellationToken cancellationToken = default)
    {
        var testResult = await _testRepository
            .GetById(TestId.Create(command.TestId), cancellationToken);

        if (testResult.IsFailure)
            return testResult.Error;
        
        var test = testResult.Value;
        
        if (test.UserId != command.UserId)
            return (ErrorList)Error.Conflict("user_id.not.match", "User id does not match test id");
        
        LimitTime? limitTime = null;

        if (command.Seconds != null && command.Minutes != null)
        {
            var limitTimeResult = LimitTime.Create(command.Seconds.Value, command.Minutes.Value);
        
            if (limitTimeResult.IsFailure)
                return (ErrorList)limitTimeResult.Error;
            
            limitTime = limitTimeResult.Value;
        }
        
        var privacySettings = PrivacySettings
            .Create(command.IsPrivate ?? true, command.UsersNamesAreAllowed ?? [], command.UsersEmailsAreAllowed ?? []);
        
        if (privacySettings.IsFailure)
            return (ErrorList)privacySettings.Error;

        var updateResult = test
            .UpdateInfo(command.TestName, command.Theme, command.WithAI, privacySettings.Value, limitTime);
        
        if (updateResult.IsFailure)
            return (ErrorList)updateResult.Error;

        if (command.TasksToCreate is not null)
        {
            var tasks = command.TasksToCreate
                .Select(t => Task.Create(
                    TaskId.AddNewId(),
                    t.TaskName,
                    t.TaskMessage,
                    t.RightAnswer,
                    t.Answers)
                )
                .ToList();

            if (tasks.Any(t => t.IsFailure))
                return (ErrorList)tasks
                    .Where(t => t.IsFailure)
                    .Select(e => e.Error)
                    .ToList();
        
            test.AddTasks(tasks.Select(t => t.Value));
        }

        if (command.TasksToUpdate is not null)
        {
            var tasks = command.TasksToUpdate
                .Select(t => Task.Create(
                    TaskId.Create(t.TaskId),
                    t.TaskName,
                    t.TaskMessage,
                    t.RightAnswer,
                    t.Answers)
                )
                .ToList();
            
            if (tasks.Any(t => t.IsFailure))
                return (ErrorList)tasks
                    .Where(t => t.IsFailure)
                    .Select(e => e.Error)
                    .ToList();
            
            test.UpdateTasks(tasks.Select(t => t.Value));
        }

        if (command.TaskIdsToDelete is not null)
        {
            var tasks = test
                .GetTasksByIds(command.TaskIdsToDelete);

            var files = tasks
                .Select(t => new FileInfo("photos", t.ImagePath));
        
            await _messageQueue.WriteAsync(files, cancellationToken);
            _testRepository.DeleteTasks(tasks);
        }
        
        _testRepository.Save(test);
        await _unitOfWork.SaveChanges(cancellationToken);

        return (Guid)test.Id;
    }
}