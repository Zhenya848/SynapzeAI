using CSharpFunctionalExtensions;
using TestsService.Application.Abstractions;
using TestsService.Application.Repositories;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Id;

namespace TestsService.Application.Tests.Commands.Delete;

public class DeleteTestHandler : ICommandHandler<Guid, Result<Guid, ErrorList>>
{
    private readonly ITestRepository _testRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTestHandler(
        ITestRepository testRepository, 
        IUnitOfWork unitOfWork)
    {
        _testRepository = testRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        Guid testId, 
        CancellationToken cancellationToken = default)
    {
        var testResult = await _testRepository
            .GetById(TestId.Create(testId), cancellationToken);
        
        var test = testResult.Value;
        
        if (testResult.IsFailure)
            return (ErrorList)Errors.General.NotFound(testId);

        _testRepository.DeleteTest(test);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        return (Guid)test.Id;
    }
}