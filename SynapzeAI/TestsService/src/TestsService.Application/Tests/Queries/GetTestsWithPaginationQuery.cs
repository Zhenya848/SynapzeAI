namespace TestsService.Application.Tests.Queries;

public record GetTestsWithPaginationQuery(
    int Page,
    int PageSize);