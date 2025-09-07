namespace TestsService.Application.SolvingHistories.Querise;

public record GetSolvingHistoriesByPaginationQuery(
    int Page, 
    int PageSize,
    Guid TestId,
    string? SearchUserName = null,
    string? SearchUserTelegram = null,
    string? OrderBy =  null);