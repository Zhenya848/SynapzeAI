using CSharpFunctionalExtensions;
using TestsService.Application.Abstractions;
using TestsService.Application.Repositories;
using TestsService.Domain;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Id;
using TestsService.Domain.ValueObjects;

namespace TestsService.Application.Tests.Commands.Create;

public class CreateTestHandler : ICommandHandler<CreateTestCommand, Result<Guid, ErrorList>>
{
    private readonly ITestRepository _testRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTestHandler(
        IUnitOfWork unitOfWork,
        ITestRepository testRepository)
    {
        _testRepository = testRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        CreateTestCommand command, 
        CancellationToken cancellationToken = default)
    {
        LimitTime? limitTime = null;

        if (command.Seconds != null && command.Minutes != null)
        {
            var limitTimeResult = LimitTime.Create(command.Seconds.Value, command.Minutes.Value);
        
            if (limitTimeResult.IsFailure)
                return (ErrorList)limitTimeResult.Error;
            
            limitTime = limitTimeResult.Value;
        }
        
        var test = Test.Create(
            TestId.AddNewId(), 
            command.UserId,
            command.TestName, 
            command.Theme,
            command.IsPublished,
            limitTime);
        
        if (test.IsFailure)
            return (ErrorList)test.Error;

        var result = _testRepository.Add(test.Value);
        
        await _unitOfWork.SaveChanges(cancellationToken);

        return result;
    }
}