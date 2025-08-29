namespace TestsService.Application.SolvingHistories.Querise;

public record GetSolvingHistoriesByPaginationQuery(
    int Page, 
    int PageSize,
    Guid TestId,
    string? SearchUserName = null,
    string? SearchUserEmail = null,
    string? OrderBy =  null);