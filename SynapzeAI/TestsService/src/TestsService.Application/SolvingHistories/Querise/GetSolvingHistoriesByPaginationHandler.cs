using System.Linq.Expressions;
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
        
        if (string.IsNullOrEmpty(query.OrderBy) == false)
        {
            Expression<Func<SolvingHistoryDto, object>> selector = query.OrderBy?.ToLower() switch
            {
                "время решения" => solvingHistory => solvingHistory.SolvingDate,

                _ => solvingHistory => 
                    solvingHistory.TaskHistories
                        .Count(th => th.RightAnswer != null && th.UserAnswer.ToLower() == th.RightAnswer.ToLower())
            };

            if (query.OrderBy.ToLower() == "по успешности прохождения (сверху вниз)")
                solvingHistoriesQuery = solvingHistoriesQuery.OrderBy(selector);
            else
                solvingHistoriesQuery = solvingHistoriesQuery.OrderByDescending(selector);
        }
        
        var totalCount = await solvingHistoriesQuery.CountAsync(cancellationToken);
        
        var solvingHistories = await solvingHistoriesQuery
            .GetItemsWithPagination(query.Page, query.PageSize)
            .Include(th => th.TaskHistories.OrderBy(sn => sn.SerialNumber))
            .ToListAsync(cancellationToken);

        return new PagedList<SolvingHistoryDto>()
        {
            Items = solvingHistories,
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }
}