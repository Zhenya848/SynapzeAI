using Core;
using CSharpFunctionalExtensions;
using TestsService.Application.Abstractions;
using TestsService.Application.Repositories;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Id;

namespace TestsService.Application.SolvingHistories.Commands.UpdateAIMessages;

public class UpdateAIMessagesForTasksHandler : ICommandHandler<UpdateAIMessagesForTasksCommand, UnitResult<ErrorList>>
{
    private readonly ITestRepository _testRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAIMessagesForTasksHandler(ITestRepository testRepository, IUnitOfWork unitOfWork)
    {
        _testRepository = testRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(
        UpdateAIMessagesForTasksCommand command, 
        CancellationToken cancellationToken = default)
    {
        
        var solvingHistoryResult = await _testRepository
            .GetSolvingHistoryById(SolvingHistoryId.Create(command.SolvingHistoryId), cancellationToken);

        if (solvingHistoryResult.IsFailure)
            return (ErrorList)solvingHistoryResult.Error;
        
        var solvingHistory = solvingHistoryResult.Value;

        var taskHistoriesBySerialNumber = solvingHistory.TaskHistories
            .ToDictionary(sn => sn.SerialNumber);
        
        foreach (var aiMessage in command.AIMessagesForTasks)
        {
            if (taskHistoriesBySerialNumber.TryGetValue(aiMessage.TaskSerialNumber, out var taskHistory))
            {
                var updateAIMessageResult = taskHistory.UpdateAIMessage(aiMessage.AIMessage);

                if (updateAIMessageResult.IsFailure)
                    return (ErrorList)updateAIMessageResult.Error;
            }
        }
        
        await _unitOfWork.SaveChanges(cancellationToken);

        return Result.Success<ErrorList>();
    }
}