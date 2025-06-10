using CSharpFunctionalExtensions;
using TestsService.Application.Abstractions;
using TestsService.Application.Repositories;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Id;
using TestsService.Domain.ValueObjects;

namespace TestsService.Application.Tests.Commands.Update;

public class UpdateTestHandler : ICommandHandler<UpdateTestCommand, Result<Guid, ErrorList>>
{
    private readonly ITestRepository _testRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTestHandler(
        IUnitOfWork unitOfWork,
        ITestRepository testRepository)
    {
        _testRepository = testRepository;
        _unitOfWork = unitOfWork;
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
        
        if (string.IsNullOrWhiteSpace(command.TestName))
            return (ErrorList)Errors.General.ValueIsRequired(command.TestName);
        
        LimitTime? limitTime = null;

        if (command.Seconds != null && command.Minutes != null && command.Hours != null)
        {
            var limitTimeResult = LimitTime.Create(command.Seconds.Value, command.Minutes.Value, command.Hours.Value);
        
            if (limitTimeResult.IsFailure)
                return (ErrorList)limitTimeResult.Error;
            
            limitTime = limitTimeResult.Value;
        }

        test.UpdateInfo(command.TestName, command.IsPublished, limitTime);
        
        _testRepository.Save(test);
        await _unitOfWork.SaveChanges(cancellationToken);

        return (Guid)test.Id;
    }
}