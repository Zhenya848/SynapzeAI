using Application.Abstractions;
using Core;
using CSharpFunctionalExtensions;
using TestsService.Application.Abstractions;
using TestsService.Application.Repositories;
using TestsService.Domain.Shared.ValueObjects.Id;

namespace TestsService.Application.SolvingHistories.Commands.Update;

public class UpdateSolvingHistoryHandler : ICommandHandler<UpdateSolvingHistoryCommand, UnitResult<ErrorList>>
{
    private readonly ITestRepository _testRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSolvingHistoryHandler(ITestRepository testRepository, IUnitOfWork unitOfWork)
    {
        _testRepository = testRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(
        UpdateSolvingHistoryCommand command, 
        CancellationToken cancellationToken = default)
    {
        
        var solvingHistoryResult = await _testRepository
            .GetSolvingHistoryById(SolvingHistoryId.Create(command.SolvingHistoryId), cancellationToken);

        if (solvingHistoryResult.IsFailure)
            return (ErrorList)solvingHistoryResult.Error;
        
        var solvingHistory = solvingHistoryResult.Value;

        var taskHistoriesBySerialNumber = solvingHistory.TaskHistories
            .ToDictionary(sn => sn.SerialNumber);
        
        foreach (var task in command.Tasks)
        {
            if (taskHistoriesBySerialNumber.TryGetValue(task.SerialNumber, out var taskHistory))
            {
                var updateMessageResult = taskHistory.UpdateMessage(task.Message);

                if (updateMessageResult.IsFailure)
                    return (ErrorList)updateMessageResult.Error;

                taskHistory.UpdatePoints(task.Points);
            }
        }
        
        await _unitOfWork.SaveChanges(cancellationToken);

        return Result.Success<ErrorList>();
    }
}