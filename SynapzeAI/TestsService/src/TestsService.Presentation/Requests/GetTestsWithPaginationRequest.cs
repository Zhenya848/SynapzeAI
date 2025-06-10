namespace TestsService.Presentation.Requests;

public record GetTestsWithPaginationRequest(
    int Page,
    int PageSize);