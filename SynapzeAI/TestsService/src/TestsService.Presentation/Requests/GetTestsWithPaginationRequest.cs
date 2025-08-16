using TestsService.Domain.Shared.ValueObjects;

namespace TestsService.Presentation.Requests;

public record GetTestsWithPaginationRequest(
    int Page,
    int PageSize,
    string? SearchTestName = null,
    string? SearchTestTheme = null,
    string? SearchUserName = null,
    string? OrderBy = null);