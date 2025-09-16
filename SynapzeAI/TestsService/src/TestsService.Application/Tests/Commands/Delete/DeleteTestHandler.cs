using Core;
using CSharpFunctionalExtensions;
using TestsService.Application.Abstractions;
using TestsService.Application.Repositories;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Id;

namespace TestsService.Application.Tests.Commands.Delete;

public class DeleteTestHandler : ICommandHandler<DeleteTestCommand, Result<Guid, ErrorList>>
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
        DeleteTestCommand command, 
        CancellationToken cancellationToken = default)
    {
        var testResult = await _testRepository
            .GetById(TestId.Create(command.TestId), cancellationToken);
        
        if (testResult.IsFailure)
            return (ErrorList)Errors.General.NotFound(command.TestId);
        
        var test = testResult.Value;
        
        if (test.UserId != command.UserId)
            return (ErrorList)Error.Conflict("user_id.not.match", "Id пользователя не сходится с Id создателя викторины");

        _testRepository.DeleteTest(test);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        return (Guid)test.Id;
    }
}