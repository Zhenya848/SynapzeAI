using TestsService.Domain.Shared.ValueObjects;

namespace TestsService.Presentation.Requests;

public record GetTestsWithPaginationRequest(
    int Page,
    int PageSize,
    string? SearchTestName = null,
    string? SearchTestTheme = null,
    string? SearchUserEmail = null,
    string? OrderBy = null,
    UserInfo? UserData = null);