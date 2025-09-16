using System.Text.Json;
using Core;
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

    public async Task<Result<Test, ErrorList>> GetById(
        TestId id,
        CancellationToken cancellationToken)
    {
        var tests = _context.Tests
            .Include(t => t.Tasks);
        
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

    public async Task<Result<SavedTest, Error>> GetSavedTest(
        TestId testId, 
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        var result = await _context.SavedTests
            .FirstOrDefaultAsync(st => st.TestId == testId && st.UserId == userId, cancellationToken);

        if (result is null)
            return Errors.General.NotFound();
        
        return result;
    }

    public Guid DeleteSavedTest(SavedTest savedTest)
    {
        _context.SavedTests.Remove(savedTest);
        _logger.LogInformation(
            "Removed saved test {test} with user id {id}", 
            savedTest.TestId.Value, 
            savedTest.UserId);
        
        return savedTest.Id;
    }

    public async Task<IEnumerable<Task>> GetTasks(
        IEnumerable<Guid> taskIds,
        CancellationToken cancellationToken = default,
        Guid? userId = null)
    {
        var resultQuery = _context.Tasks
            .Where(i => taskIds.Contains(i.Id))
            .Include(ts => ts.TaskStatistics.Where(ui => ui.UserId == userId));
        
        var result = await resultQuery.ToListAsync(cancellationToken);
        
        return result;
    }

    public async Task<Result<SolvingHistory, Error>> GetSolvingHistoryById(
        SolvingHistoryId id, 
        CancellationToken cancellationToken = default)
    {
        var result = await _context.SolvingHistories
            .Include(th => th.TaskHistories)
            .FirstOrDefaultAsync(sh => sh.Id == id, cancellationToken);
        
        if (result == null)
            return Errors.General.NotFound(id.Value);
        
        return result;
    }
}