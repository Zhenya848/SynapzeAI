namespace TestsService.Presentation.Requests;

public record GetSolvingHistoriesByPaginationRequest(
    int Page, 
    int PageSize,
    string? SearchUserEmail = null);