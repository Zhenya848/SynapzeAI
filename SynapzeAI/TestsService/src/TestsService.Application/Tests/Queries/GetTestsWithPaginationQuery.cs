using TestsService.Domain.Shared.ValueObjects;

namespace TestsService.Application.Tests.Queries;

public record GetTestsWithPaginationQuery(
    int Page,
    int PageSize,
    string? SearchTestName = null,
    string? SearchTestTheme = null,
    string? SearchUserName = null,
    string? OrderBy = null);