namespace TestsService.Presentation.Requests;

public record GetSolvingHistoriesByPaginationRequest(
    int Page, 
    int PageSize,
    string? SearchUserName = null,
    string? SearchUserEmail = null,
    string? OrderBy =  null);