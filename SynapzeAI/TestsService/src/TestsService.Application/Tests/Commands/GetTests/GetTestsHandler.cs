using Application.Abstractions;
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
            .Where(ui => ui.UserId == userId)
            .Include(st => st.SavedTests.Where(ui => ui.UserId == userId))
            .Include(t => t.Tasks.OrderBy(sn => sn.SerialNumber))
            .ThenInclude(ts => ts.TaskStatistics.Where(ui => ui.UserId == userId));

        var tests = await testsQuery.ToListAsync(cancellationToken);
        
        tests.ForEach(test =>
        {
            test.IsSaved = test.SavedTests.Any();

            foreach (var task in test.Tasks)
                task.TaskStatistic = task.TaskStatistics.FirstOrDefault();
        });
        
        return tests;
    }
}