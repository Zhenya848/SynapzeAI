using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

namespace TestsService.Application.Repositories;

public interface IReadDbContext
{
    public IQueryable<TestDto> Tests { get; }
    public IQueryable<TaskDto> Tasks { get; }
    public IQueryable<SolvingHistoryDto> SolvingHistories { get; }
    public IQueryable<TaskStatisticDto> TaskStatistics { get; }
    public IQueryable<TaskHistoryDto> TaskHistories { get; }
}