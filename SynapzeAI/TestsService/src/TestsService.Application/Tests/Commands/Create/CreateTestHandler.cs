using Application.Abstractions;
using Core;
using CSharpFunctionalExtensions;
using TestsService.Application.Abstractions;
using TestsService.Application.Repositories;
using TestsService.Domain;
using TestsService.Domain.Shared.ValueObjects.Id;
using TestsService.Domain.ValueObjects;
using Task = TestsService.Domain.Task;

namespace TestsService.Application.Tests.Commands.Create;

public class CreateTestHandler : ICommandHandler<CreateTestCommand, Result<Guid, ErrorList>>
{
    private readonly ITestRepository _testRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGreeterService _greeterService;

    public CreateTestHandler(
        IUnitOfWork unitOfWork,
        ITestRepository testRepository,
        IGreeterService greeterService)
    {
        _testRepository = testRepository;
        _unitOfWork = unitOfWork;
        _greeterService = greeterService;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        CreateTestCommand command, 
        CancellationToken cancellationToken = default)
    {
        LimitTime? limitTime = null;

        if (command.Seconds is not null || command.Minutes is not null)
        {
            var limitTimeResult = LimitTime
                .Create(command.Seconds ?? 0, command.Minutes ?? 0);
        
            if (limitTimeResult.IsFailure)
                return (ErrorList)limitTimeResult.Error;
            
            limitTime = limitTimeResult.Value;
        }

        var tasks = command.Tasks?
            .Select(t => Task.Create(
                TaskId.AddNewId(),
                t.TaskName,
                t.TaskMessage,
                t.RightAnswer,
                t.Answers))
            .ToList();
        
        if (tasks is not null && tasks.Any(t => t.IsFailure))
            return (ErrorList)tasks.Select(t => t.Error).ToList();
        
        var test = Test.Create(
            TestId.AddNewId(), 
            command.UserId,
            command.UniqueUserName,
            command.TestName, 
            command.Theme,
            command.IsPublished,
            limitTime,
            tasks?.Select(t => t.Value));
        
        if (test.IsFailure)
            return (ErrorList)test.Error;

        var result = _testRepository.Add(test.Value);
        
        await _unitOfWork.SaveChanges(cancellationToken);

        return result;
    }
}