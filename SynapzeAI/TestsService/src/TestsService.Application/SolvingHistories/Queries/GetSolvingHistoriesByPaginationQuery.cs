namespace TestsService.Application.SolvingHistories.Queries;

public record GetSolvingHistoriesByPaginationQuery(
    int Page, 
    int PageSize,
    Guid TestId,
    string? SearchUserEmail = null);