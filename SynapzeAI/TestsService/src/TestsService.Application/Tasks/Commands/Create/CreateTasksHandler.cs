using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using TestsService.Application.Abstractions;
using TestsService.Application.Repositories;
using TestsService.Domain;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Id;
using TestsService.Domain.ValueObjects;
using Task = TestsService.Domain.Task;

namespace TestsService.Application.Tasks.Commands.Create;

public class CreateTasksHandler : ICommandHandler<CreateTasksCommand, Result<IEnumerable<Guid>, ErrorList>>
{
    private readonly ITestRepository _testRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTasksHandler(
        ITestRepository testRepository, 
        IUnitOfWork unitOfWork)
    {
        _testRepository = testRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<IEnumerable<Guid>, ErrorList>> Handle(
        CreateTasksCommand command, 
        CancellationToken cancellationToken = default)
    {
        var testResult = await _testRepository
            .GetById(TestId.Create(command.TestId), cancellationToken);

        if (testResult.IsFailure)
            return testResult.Error;
        
        var test = testResult.Value;
        
        var tasksResult = command.Tasks
            .Select(t => Task.Create(
                    TaskId.AddNewId(),
                    t.TaskName,
                    t.TaskMessage,
                    PriorityNumber.Create().Value,
                    t.RightAnswer,
                    t.Answers)
            )
            .ToList();

        if (tasksResult.Any(t => t.IsFailure))
            return (ErrorList)tasksResult.Select(e => e.Error);

        var tasks = tasksResult.Select(v => v.Value).ToList();
        
        test.AddTasks(tasks);
        _testRepository.Save(test);
        
        await _unitOfWork.SaveChanges(cancellationToken);

        return Result.Success<IEnumerable<Guid>, ErrorList>(tasks.Select(i => (Guid)i.Id));
    }
}