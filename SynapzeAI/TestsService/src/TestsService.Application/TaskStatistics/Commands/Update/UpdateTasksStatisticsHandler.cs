using Application.Abstractions;
using Core;
using CSharpFunctionalExtensions;
using TestsService.Application.Abstractions;
using TestsService.Application.Repositories;
using TestsService.Domain;

namespace TestsService.Application.TaskStatistics.Commands.Update;

public class UpdateTasksStatisticsHandler : ICommandHandler<UpdateTasksStatisticsCommand, UnitResult<ErrorList>>
{
    private readonly ITestRepository _testRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTasksStatisticsHandler(
        ITestRepository testRepository, 
        IUnitOfWork unitOfWork)
    {
        _testRepository = testRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(
        UpdateTasksStatisticsCommand command, 
        CancellationToken cancellationToken = default)
    {
        var tasks = await _testRepository
            .GetTasks(command.TasksStatistics.Select(t => t.TaskId), cancellationToken, command.UserId);
        
        var dict = command.TasksStatistics.ToDictionary(t => t.TaskId);

        foreach (var task in tasks)
        {
            dict.TryGetValue(task.Id, out var taskStatistic);
            
            if (task.TaskStatistics.Count > 0)
            {
                var updateResult = task.TaskStatistics[0].Update(
                    taskStatistic!.ErrorsCount,
                    taskStatistic.RightAnswersCount,
                    taskStatistic.LastReviewTime,
                    taskStatistic.AvgTimeSolvingSec);

                if (updateResult.IsFailure)
                    return (ErrorList)updateResult.Error;
            }
            else
            {
                var taskStatisticResult = TaskStatistic.Create(
                    command.UserId,
                    taskStatistic!.ErrorsCount,
                    taskStatistic.RightAnswersCount,
                    taskStatistic.LastReviewTime,
                    taskStatistic.AvgTimeSolvingSec);

                if (taskStatisticResult.IsFailure)
                    return (ErrorList)taskStatisticResult.Error;

                task.AddTaskStatistic(taskStatisticResult.Value);
            }
        }
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        return new UnitResult<ErrorList>();
    }
}