using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TestsService.Application.Abstractions;
using TestsService.Application.Extensions;
using TestsService.Application.Models.Dtos;
using TestsService.Application.Providers;
using TestsService.Application.Repositories;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
            var userTests = await testsQuery
                .Where(t => (t.IsPublished && t.UniqueUserName.ToLower().Contains(query.SearchUserName.ToLower())) 
                            || (t.IsPublished == false && t.UniqueUserName.ToLower() == query.SearchUserName.ToLower()))
                .GetItemsWithPagination(query.Page, query.PageSize)
                .ToListAsync(cancellationToken);
            
            await SetTasks(userTests, cancellationToken);
    
            return new PagedList<TestDto>()
            {
                Items = userTests,
                TotalCount = userTests.Count,
                Page = query.Page,
                PageSize = query.PageSize,
            };
        }
        
        testsQuery = testsQuery
            .Where(t => t.IsPublished)
            .GetItemsWithPagination(query.Page, query.PageSize);

        var tests = await testsQuery.ToListAsync(cancellationToken);
        
        await SetTasks(tests, cancellationToken);

        var result = new PagedList<TestDto>()
        {
            Items = tests,
            TotalCount = tests.Count,
            Page = query.Page,
            PageSize = query.PageSize,
        };
        
        return result;
    }

    private async Task SetTasks(List<TestDto> tests, CancellationToken cancellationToken)
    {
        var testIds = tests.Select(i => i.Id);
        
        var tasks = await _readDbContext.Tasks
            .Where(t => testIds.Contains(t.TestId))
            .ToListAsync(cancellationToken);
        
        tests.ForEach(test => test.Tasks = tasks
            .Where(t => t.TestId == test.Id)
            .ToArray());
    }
}