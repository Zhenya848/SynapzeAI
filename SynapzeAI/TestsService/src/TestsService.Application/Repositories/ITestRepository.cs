using Core;
using CSharpFunctionalExtensions;
using TestsService.Domain;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;
using TestsService.Domain.Shared.ValueObjects.Id;
using Task = TestsService.Domain.Task;

namespace TestsService.Application.Repositories;

public interface ITestRepository
{
    public Guid Add(Test client);
    
    public Task<Result<Test, ErrorList>> GetById(
        TestId id,
        CancellationToken cancellationToken = default);
    
    public Guid DeleteTest(Test test);
    public IEnumerable<Guid> DeleteTasks(IEnumerable<Task> tasks);

    public Task<Result<SavedTest, Error>> GetSavedTest(
        TestId testId,
        Guid userId,
        CancellationToken cancellationToken = default);
    public Guid DeleteSavedTest(SavedTest savedTest);

    public Task<IEnumerable<Task>> GetTasks(
        IEnumerable<Guid> taskIds,
        CancellationToken cancellationToken = default,
        Guid? userId = null);
    
    public Task<Result<SolvingHistory, Error>> GetSolvingHistoryById(
        SolvingHistoryId id,
        CancellationToken cancellationToken = default);
}