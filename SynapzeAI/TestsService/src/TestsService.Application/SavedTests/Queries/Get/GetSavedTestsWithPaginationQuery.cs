namespace TestsService.Application.SavedTests.Queries.Get;

public record GetSavedTestsWithPaginationQuery(
    Guid UserId,
    int Page,
    int PageSize,
    string? SearchTestName = null,
    string? SearchTestTheme = null,
    string? SearchUserName = null,
    string? OrderBy = null);