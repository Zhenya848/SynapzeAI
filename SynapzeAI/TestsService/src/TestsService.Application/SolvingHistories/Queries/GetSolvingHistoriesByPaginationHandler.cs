using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TestsService.Application.Abstractions;
using TestsService.Application.Extensions;
using TestsService.Application.Providers;
using TestsService.Application.Repositories;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

namespace TestsService.Application.SolvingHistories.Queries;

public class GetSolvingHistoriesByPaginationHandler : 
    IQueryHandler<GetSolvingHistoriesByPaginationQuery, PagedList<SolvingHistoryDto>>
{
    private readonly IReadDbContext _readDbContext;
    private readonly IAccountsProvider _accountsProvider;

    public GetSolvingHistoriesByPaginationHandler(
        IReadDbContext readDbContext,
        IAccountsProvider accountsProvider)
    {
        _readDbContext = readDbContext;
        _accountsProvider = accountsProvider;
    }
    
    public async Task<PagedList<SolvingHistoryDto>> Handle(
        GetSolvingHistoriesByPaginationQuery query, 
        CancellationToken cancellationToken = default)
    {
        var solvingHistoriesQuery = _readDbContext.SolvingHistories
            .Where(ti => ti.TestId == query.TestId);

        if (string.IsNullOrEmpty(query.SearchUserEmail) == false)
        {
            var userResult = await _accountsProvider
                .GetUserByEmail(query.SearchUserEmail, cancellationToken);

            if (userResult.IsFailure)
                return new PagedList<SolvingHistoryDto>()
                {
                    Items = [],
                    TotalCount = 0,
                    Page = query.Page,
                    PageSize = query.PageSize,
                };
            
            var user = userResult.Value;
            
            var userSolvingHistories = await solvingHistoriesQuery
                .Where(ui => ui.UserId == user.Id)
                .GetItemsWithPagination(query.Page, query.PageSize)
                .ToListAsync(cancellationToken);
            
            userSolvingHistories.ForEach(sh => sh.User = user);
            
            return new PagedList<SolvingHistoryDto>()
            {
                Items = userSolvingHistories,
                TotalCount = userSolvingHistories.Count,
                Page = query.Page,
                PageSize = query.PageSize,
            };
        }
        
        solvingHistoriesQuery = solvingHistoriesQuery
            .GetItemsWithPagination(query.Page, query.PageSize);
        
        var solvingHistories = await solvingHistoriesQuery.ToListAsync(cancellationToken);
        var userIds = solvingHistories.Select(ui => ui.UserId);
        
        var usersResult = await _accountsProvider
            .GetUsers(userIds, cancellationToken);
        
        if (usersResult.IsFailure)
            return new PagedList<SolvingHistoryDto>()
            {
                Items = [],
                TotalCount = 0,
                Page = query.Page,
                PageSize = query.PageSize,
            };
        
        var users = usersResult.Value;
        
        solvingHistories.ForEach(sh => sh.User = users!.First(i => i.Id == sh.UserId));
        
        return new PagedList<SolvingHistoryDto>()
        {
            Items = solvingHistories,
            TotalCount = solvingHistories.Count,
            Page = query.Page,
            PageSize = query.PageSize,
        };
    }
}