using CSharpFunctionalExtensions;
using TestsService.Application.Abstractions;
using TestsService.Application.Repositories;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Id;

namespace TestsService.Application.SavedTests.Commands.Delete;

public class DeleteSavedTestHandler : ICommandHandler<DeleteSavedTestCommand, Result<Guid, ErrorList>>
{
    private readonly ITestRepository _testRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSavedTestHandler(ITestRepository testRepository, IUnitOfWork unitOfWork)
    {
        _testRepository = testRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteSavedTestCommand command, 
        CancellationToken cancellationToken = default)
    {
        var savedTestResult = await _testRepository
            .GetSavedTest(TestId.Create(command.TestId), command.UserId, cancellationToken);

        if (savedTestResult.IsFailure)
            return (ErrorList)savedTestResult.Error;
        
        var savedTest = savedTestResult.Value;
        
        var result = _testRepository.DeleteSavedTest(savedTest);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        return result;
    }
}