using CSharpFunctionalExtensions;
using TestsService.Application.Abstractions;
using TestsService.Application.Repositories;
using TestsService.Domain;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Id;
using TestsService.Domain.ValueObjects;

namespace TestsService.Application.SolvingHistories.Commands.Create;

public class AddSolvingHistoryHandler : ICommandHandler<AddSolvingHistoryCommand, UnitResult<ErrorList>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITestRepository _testRepository;

    public AddSolvingHistoryHandler(IUnitOfWork unitOfWork, ITestRepository testRepository)
    {
        _unitOfWork = unitOfWork;
        _testRepository = testRepository;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(
        AddSolvingHistoryCommand command, 
        CancellationToken cancellationToken = default)
    {
        var testResult = await _testRepository
            .GetById(TestId.Create(command.TestId), cancellationToken);
        
        if (testResult.IsFailure)
            return testResult.Error;
        
        var test = testResult.Value;

        var taskHistoriesResult = command.TaskHistories
            .Select(th => TaskHistory.Create(
                th.TaskName,
                th.TaskMessage,
                th.UserAnswer,
                th.RightAnswer,
                th.Answers,
                th.MessageAI,
                th.ImagePath,
                th.AudioPath))
            .ToList();

        if (taskHistoriesResult.Any(th => th.IsFailure))
        {
            var errors = taskHistoriesResult
                .Where(th => th.IsFailure)
                .Select(th => th.Error)
                .ToList();

            return (ErrorList)errors;
        }

        var taskHistories = taskHistoriesResult
            .Select(th => th.Value);

        var solvingHistoriesResult = SolvingHistory
            .Create(taskHistories, command.SolvingDate, command.SolvingTimeSeconds);
        
        if (solvingHistoriesResult.IsFailure)
            return (ErrorList)solvingHistoriesResult.Error;
        
        test.AddSolvingHistory(solvingHistoriesResult.Value);
        await _unitOfWork.SaveChanges(cancellationToken);

        return Result.Success<ErrorList>();
    }
}