using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TestsService.Application.Abstractions;
using TestsService.Application.Repositories;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

namespace TestsService.Application.Tests.Queries;

public class GetTestsWithPaginationHandler : IQueryHandler<GetTestsWithPaginationQuery, PagedList<TestDto>>
{
    private readonly IReadDbContext _readDbContext;

    public GetTestsWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    
    public async Task<PagedList<TestDto>> Handle(
        GetTestsWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var testsQuery = _readDbContext.Tests;
        
        if (string.IsNullOrEmpty(query.SearchTestName) == false)
            testsQuery = testsQuery.Where(t => t.TestName.Contains(query.SearchTestName));
        
        if (string.IsNullOrEmpty(query.SearchTestTheme) == false)
            testsQuery = testsQuery.Where(t => t.Theme.Contains(query.SearchTestTheme));

        if (string.IsNullOrEmpty(query.OrderBy) == false)
        {
            Expression<Func<TestDto, object>> selector = query.OrderBy?.ToLower() switch
            {
                "название" => test => test.TestName,
                "тема" => test => test.Theme,

                _ => test => test.TestName
            };

            testsQuery = testsQuery.OrderBy(selector);
        }

        if (string.IsNullOrEmpty(query.SearchUserName) == false)
        {
            var userTestsQuery = testsQuery
                .Where(t => (t.IsPublished && t.UniqueUserName.ToLower().Contains(query.SearchUserName.ToLower())) 
                    || (t.IsPublished == false && t.UniqueUserName.ToLower() == query.SearchUserName.ToLower()));
            
            var userTestsCount = await userTestsQuery.CountAsync(cancellationToken);
            var userTests = await GetTestListByQuery(userTestsQuery, query, cancellationToken);
    
            return new PagedList<TestDto>()
            {
                Items = userTests,
                TotalCount = userTestsCount,
                Page = query.Page,
                PageSize = query.PageSize,
            };
        }
        
        testsQuery = testsQuery
            .Where(t => t.IsPublished);

        var testsCount = await testsQuery.CountAsync(cancellationToken);
        var tests = await GetTestListByQuery(testsQuery, query, cancellationToken);
        
        return new PagedList<TestDto>()
        {
            Items = tests,
            TotalCount = testsCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    private async Task<List<TestDto>> GetTestListByQuery(
        IQueryable<TestDto> query,
        GetTestsWithPaginationQuery queryParams,
        CancellationToken cancellationToken = default)
    {
        var tests = await query
            .GetItemsWithPagination(queryParams.Page, queryParams.PageSize)
            .Include(st => st.SavedTests.Where(ui => ui.UserId == queryParams.UserId))
            .Include(t => t.Tasks.OrderBy(sn => sn.SerialNumber))
            .ThenInclude(ts => ts.TaskStatistics.Where(ui => ui.UserId == queryParams.UserId))
            .ToListAsync(cancellationToken);

        if (queryParams.UserId is not null)
        {
            tests.ForEach(test =>
            {
                test.IsSaved = test.SavedTests.Any();

                foreach (var task in test.Tasks)
                    task.TaskStatistic = task.TaskStatistics.FirstOrDefault();
            });
        }
        
        return tests;
    }
}