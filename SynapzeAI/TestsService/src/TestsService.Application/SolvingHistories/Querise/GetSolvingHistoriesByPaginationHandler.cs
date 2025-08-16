using Microsoft.EntityFrameworkCore;
using TestsService.Application.Abstractions;
using TestsService.Application.Extensions;
using TestsService.Application.Repositories;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

namespace TestsService.Application.SolvingHistories.Querise;

public class GetSolvingHistoriesByPaginationHandler : 
    IQueryHandler<GetSolvingHistoriesByPaginationQuery, PagedList<SolvingHistoryDto>>
{
    private readonly IReadDbContext _readDbContext;

    public GetSolvingHistoriesByPaginationHandler(
        IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    
    public async Task<PagedList<SolvingHistoryDto>> Handle(
        GetSolvingHistoriesByPaginationQuery query, 
        CancellationToken cancellationToken = default)
    {
        var solvingHistoriesQuery = _readDbContext.SolvingHistories
            .Where(ti => ti.TestId == query.TestId);

        if (string.IsNullOrEmpty(query.SearchUserEmail) == false)
        {
            solvingHistoriesQuery = solvingHistoriesQuery
                .Where(u => u.UserEmail.ToLower().Contains(query.SearchUserEmail.ToLower()));
        }
        
        if (string.IsNullOrEmpty(query.SearchUserName) == false)
        {
            solvingHistoriesQuery = solvingHistoriesQuery
                .Where(u => u.UniqueUserName.ToLower().Contains(query.SearchUserName.ToLower()));
        }
        
        solvingHistoriesQuery = solvingHistoriesQuery
            .GetItemsWithPagination(query.Page, query.PageSize);
        
        var solvingHistories = await solvingHistoriesQuery.ToListAsync(cancellationToken);

        return new PagedList<SolvingHistoryDto>()
        {
            Items = solvingHistories,
            TotalCount = solvingHistories.Count,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }
}