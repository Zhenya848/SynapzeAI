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
    
    public Guid Save(Test test);
    
    public Task<Result<Test, ErrorList>> GetById(
        TestId id,
        CancellationToken cancellationToken);
    
    public Guid DeleteTest(Test test);
    public IEnumerable<Guid> DeleteTasks(IEnumerable<Task> tasks);
    public IEnumerable<Guid> DeleteSolvingHistories(IEnumerable<SolvingHistory> solvingHistories);
}