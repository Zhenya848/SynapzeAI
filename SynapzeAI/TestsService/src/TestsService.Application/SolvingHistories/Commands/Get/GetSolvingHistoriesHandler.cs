using Microsoft.EntityFrameworkCore;
using TestsService.Application.Abstractions;
using TestsService.Application.Repositories;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Dtos;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

namespace TestsService.Application.SolvingHistories.Commands.Get;

public class GetSolvingHistoriesHandler : ICommandHandler<Guid, IEnumerable<SolvingHistoryDto>>
{
    private readonly IReadDbContext _readDbContext;

    public GetSolvingHistoriesHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    
    public async Task<IEnumerable<SolvingHistoryDto>> Handle(
        Guid testId, 
        CancellationToken cancellationToken = default)
    {
        var result = await _readDbContext.SolvingHistories
            .Where(sh => sh.TestId == testId)
            .ToListAsync(cancellationToken);
        
        return result;
    }
}