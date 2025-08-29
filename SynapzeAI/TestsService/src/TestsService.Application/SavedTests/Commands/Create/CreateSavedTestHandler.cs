using CSharpFunctionalExtensions;
using TestsService.Application.Abstractions;
using TestsService.Application.Repositories;
using TestsService.Domain;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Id;

namespace TestsService.Application.SavedTests.Commands.Create;

public class CreateSavedTestHandler : ICommandHandler<CreateSavedTestCommand, Result<Guid, ErrorList>>
{
    private readonly ITestRepository _testRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSavedTestHandler(ITestRepository testRepository, IUnitOfWork unitOfWork)
    {
        _testRepository = testRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        CreateSavedTestCommand command, 
        CancellationToken cancellationToken = default)
    {
        var testResult = await _testRepository
            .GetById(TestId.Create(command.TestId), cancellationToken);

        if (testResult.IsFailure)
            return testResult.Error;
        
        var test = testResult.Value;
        var savedTest = new SavedTest(SavedTestId.AddNewId(), command.UserId);
        
        test.AddSavedTest(savedTest);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        return (Guid)savedTest.Id;
    }
}