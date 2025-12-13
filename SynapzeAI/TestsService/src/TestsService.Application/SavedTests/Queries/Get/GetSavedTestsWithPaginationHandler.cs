using System.Linq.Expressions;
using Application.Abstractions;
using Application.Pagination;
using Microsoft.EntityFrameworkCore;
using TestsService.Application.Abstractions;
using TestsService.Application.Repositories;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;
using QueriesExtensions = Application.Abstractions.QueriesExtensions;

namespace TestsService.Application.SavedTests.Queries.Get;

public class GetSavedTestsWithPaginationHandler 
    : IQueryHandler<GetSavedTestsWithPaginationQuery, PagedList<TestDto>>
{
    private readonly IReadDbContext _readDbContext;

    public GetSavedTestsWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    
    public async Task<PagedList<TestDto>> Handle(
        GetSavedTestsWithPaginationQuery query, 
        CancellationToken cancellationToken = default)
    {
        var tests = _readDbContext.Tests
            .Include(st => st.SavedTests.Where(ui => ui.UserId == query.UserId))
            .Include(t => t.Tasks)
            .ThenInclude(ts => ts.TaskStatistics.Where(ui => ui.UserId == query.UserId))
            .Where(st => st.SavedTests.Any(ui => ui.UserId == query.UserId));
        
        if (string.IsNullOrEmpty(query.SearchTestName) == false)
            tests = tests.Where(t => t.TestName.Contains(query.SearchTestName));
        
        if (string.IsNullOrEmpty(query.SearchTestTheme) == false)
            tests = tests.Where(t => t.Theme.Contains(query.SearchTestTheme));

        if (string.IsNullOrEmpty(query.OrderBy) == false)
        {
            Expression<Func<TestDto, object>> selector = query.OrderBy?.ToLower() switch
            {
                "название" => test => test.TestName,
                "тема" => test => test.Theme,

                _ => test => test.TestName
            };

            tests =tests.OrderBy(selector);
        }

        if (string.IsNullOrEmpty(query.SearchUserName) == false)
        {
            tests = tests
                .Where(u => u.UniqueUserName.ToLower().Contains(query.SearchUserName.ToLower()));
        }
        
        var totalCount = await tests.CountAsync(cancellationToken);

        var result = await QueriesExtensions.GetItemsWithPagination(tests, query.Page, query.PageSize)
            .ToListAsync(cancellationToken);
        
        result.ForEach(test =>
        {
            test.IsSaved = test.SavedTests.Any();

            foreach (var task in test.Tasks)
                task.TaskStatistic = task.TaskStatistics.FirstOrDefault();
        });

        return new PagedList<TestDto>()
        {
            Page = query.Page,
            PageSize = query.PageSize,
            Items = result,
            TotalCount = totalCount
        };
    }
}