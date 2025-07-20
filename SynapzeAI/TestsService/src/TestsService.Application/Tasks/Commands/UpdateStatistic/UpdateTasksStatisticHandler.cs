using CSharpFunctionalExtensions;
using TestsService.Application.Abstractions;
using TestsService.Application.Repositories;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Dtos;
using TestsService.Domain.Shared.ValueObjects.Id;
using TestsService.Domain.ValueObjects;

namespace TestsService.Application.Tasks.Commands.UpdateStatistic;

public class UpdateTasksStatisticHandler : ICommandHandler<UpdateTasksStatisticCommand, UnitResult<ErrorList>>
{
    private readonly ITestRepository _testRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTasksStatisticHandler(
        ITestRepository testRepository, 
        IUnitOfWork unitOfWork)
    {
        _testRepository = testRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(
        UpdateTasksStatisticCommand command, 
        CancellationToken cancellationToken = default)
    {
        var testResult = await _testRepository
            .GetById(TestId.Create(command.TestId), cancellationToken);

        if (testResult.IsFailure)
            return testResult.Error;

        foreach (var task in command.Tasks)
        {
            var taskStatisticResult = TaskStatistic.Create(
                    task.Statistic.ErrorsCount, 
                    task.Statistic.RightAnswersCount, 
                    task.Statistic.LastReviewTime, 
                    task.Statistic.AvgTimeSolvingSec);
            
            if (taskStatisticResult.IsFailure)
                return (ErrorList)taskStatisticResult.Error;
            
            testResult.Value.Tasks
                .FirstOrDefault(i => i.Id == task.TaskId)?
                .UpdateStatistic(taskStatisticResult.Value);
        }
        
        await _unitOfWork.SaveChanges(cancellationToken);

        return Result.Success<ErrorList>();
    }
}