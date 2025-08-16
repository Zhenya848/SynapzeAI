using System.Text.Json;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestsService.Application.Repositories;
using TestsService.Domain;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;
using TestsService.Domain.Shared.ValueObjects.Id;
using TestsService.Infrastructure.DbContexts;
using Task = TestsService.Domain.Task;

namespace TestsService.Infrastructure.Repositories;

public class TestRepository : ITestRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<TestRepository> _logger;

    public TestRepository(
        AppDbContext context,
        ILogger<TestRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public Guid Add(Test test)
    {
        _context.Tests.Add(test);
        _logger.LogInformation("Created test {test} with id {id}", test.TestName, test.Id.Value);
        
        return test.Id;
    }

    public Guid Save(Test test)
    {
        _context.Tests.Attach(test);
        _logger.LogInformation("Updated test {test} with id {id}", test.TestName, test.Id.Value);
        
        return test.Id;
    }

    public async Task<Result<Test, ErrorList>> GetById(
        TestId id,
        CancellationToken cancellationToken)
    {
        var tests = _context.Tests
            .Include(t => t.Tasks)
            .Include(sh => sh.SolvingHistories);
        
        var testResult = await tests
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

        if (testResult == null)
            return (ErrorList)Errors.General.NotFound(id.Value);

        return testResult;
    }

    public Guid DeleteTest(Test test)
    {
        _context.Tests.Remove(test);
        _logger.LogInformation("Deleted test {test} with id {id}", test.TestName, test.Id.Value);
        
        return test.Id;
    }

    public IEnumerable<Guid> DeleteTasks(IEnumerable<Task> tasks)
    {
        _context.Tasks.RemoveRange(tasks);

        string taskNames = string.Join(", ", tasks.Select(t => t.TaskName));
        var taskIds = tasks.Select(t => t.Id.Value);
        
        _logger.LogInformation("Deleted tasks {tasks} with ids {id}", taskNames, string.Join(", ", taskIds));
        
        return taskIds;
    }

    public IEnumerable<Guid> DeleteSolvingHistories(IEnumerable<SolvingHistory> solvingHistories)
    {
        _context.SolvingHistories.RemoveRange(solvingHistories);
        
        string solvingHistoriesNames = string.Join(", ", solvingHistories.Select(s => s.Id));
        var solvingHistoriesIds = solvingHistories.Select(s => s.Id.Value);
        
        _logger.LogInformation(
            "Deleted solving histories {solvingHistories} with ids {id}", 
            solvingHistoriesNames, 
            string.Join(", ", solvingHistoriesIds));
        
        return solvingHistoriesIds;
    }
}