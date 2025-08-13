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

public class GetTestsWithPaginationHandler : IQueryHandler<GetTestsWithPaginationQuery, PagedList<GlobalTestDto>>
{
    private readonly IReadDbContext _readDbContext;
    private readonly IAccountsProvider _accountsProvider;
    private readonly ITestRepository _testRepository;

    public GetTestsWithPaginationHandler(
        IReadDbContext readDbContext,
        IAccountsProvider accountsProvider,
        ITestRepository testRepository)
    {
        _readDbContext = readDbContext;
        _accountsProvider = accountsProvider;
        _testRepository = testRepository;
    }
    
    public async Task<PagedList<GlobalTestDto>> Handle(
        GetTestsWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var testsQuery = _readDbContext.Tests
            .Where(t => t.PrivacySettings.IsPrivate == false);
        
        if (string.IsNullOrEmpty(query.SearchTestName) == false)
            testsQuery = testsQuery.Where(t => t.TestName.Contains(query.SearchTestName));
        
        if (string.IsNullOrEmpty(query.SearchTestTheme) == false)
            testsQuery = testsQuery.Where(t => t.Theme.Contains(query.SearchTestTheme));
        
        testsQuery = testsQuery
            .Where(t => t.PrivacySettings.IsPrivate == false);
        
        testsQuery = _testRepository.GetAllowedTestsForUser(testsQuery, query.UserData);

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

        if (string.IsNullOrEmpty(query.SearchUserEmail) == false)
        {
            try
            {
                var userResult = await _accountsProvider
                    .GetUserByEmail(query.SearchUserEmail, cancellationToken);

                if (userResult.IsFailure)
                    return new PagedList<GlobalTestDto>()
                    {
                        Items = [],
                        TotalCount = 0,
                        Page = query.Page,
                        PageSize = query.PageSize,
                    };
                
                var user = userResult.Value;
                
                var userTests = await testsQuery
                    .Where(ui => ui.UserId == user.Id)
                    .GetItemsWithPagination(query.Page, query.PageSize)
                    .ToListAsync(cancellationToken);

                var userTestsResult = userTests
                    .Select(t => new GlobalTestDto(t, user))
                    .ToList();
        
                return new PagedList<GlobalTestDto>()
                {
                    Items = userTestsResult,
                    TotalCount = userTestsResult.Count,
                    Page = query.Page,
                    PageSize = query.PageSize,
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        testsQuery = testsQuery
            .GetItemsWithPagination(query.Page, query.PageSize);

        var tests = await testsQuery.ToListAsync(cancellationToken);
        var testIds = tests.Select(i => i.Id);
        
        var tasks = await _readDbContext.Tasks
            .Where(t => testIds.Contains(t.TestId))
            .ToListAsync(cancellationToken);
        
        tests.ForEach(test => test.Tasks = tasks
            .Where(t => t.TestId == test.Id)
            .ToArray());
        
        var userIds = tests.Select(i => i.UserId).Distinct();

        var usersResult = await _accountsProvider
            .GetUsers(userIds, cancellationToken);
        
        if (usersResult.IsFailure)
            return new PagedList<GlobalTestDto>()
            {
                Items = [],
                TotalCount = 0,
                Page = query.Page,
                PageSize = query.PageSize,
            };

        var users = usersResult.Value;

        var items = tests
            .Select(t => new GlobalTestDto(t, users!.First(u => u.Id == t.UserId)))
            .ToList();

        var result = new PagedList<GlobalTestDto>()
        {
            Items = items,
            TotalCount = tests.Count,
            Page = query.Page,
            PageSize = query.PageSize,
        };
        
        return result;
    }
}