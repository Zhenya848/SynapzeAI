namespace TestsService.Presentation.Requests;

public record CreateTestRequest(
    string TestName,
    string Theme,
    bool IsPublished,
    int? Seconds,
    int? Minutes);