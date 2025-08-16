using Microsoft.EntityFrameworkCore;
using TestsService.Application.Abstractions;
using TestsService.Application.Repositories;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

namespace TestsService.Application.Tests.Commands.GetTests;

public class GetTestsHandler : IQueryHandler<Guid, IEnumerable<TestDto>>
{
    private readonly IReadDbContext _readDbContext;

    public GetTestsHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    
    public async Task<IEnumerable<TestDto>> Handle(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        var testsQuery = _readDbContext.Tests
            .Where(ui => ui.UserId == userId);

        var tests = await testsQuery.ToListAsync(cancellationToken);
        var testIds = tests.Select(i => i.Id);
        
        var tasks = await _readDbContext.Tasks
            .Where(t => testIds.Contains(t.TestId))
            .ToListAsync(cancellationToken);
        
        tests.ForEach(test => test.Tasks = tasks
            .Where(t => t.TestId == test.Id)
            .OrderBy(t => t.SerialNumber)
            .ToArray());
        
        return tests;
    }
}