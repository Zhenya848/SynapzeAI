namespace TestsService.Presentation.Requests;

public record GetSolvingHistoriesByPaginationRequest(
    int Page, 
    int PageSize,
    string? SearchUserName = null,
    string? SearchUserTelegram = null,
    string? OrderBy =  null);