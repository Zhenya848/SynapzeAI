namespace TestsService.Presentation.Requests;

public record CreateTestRequest(
    string TestName,
    bool IsPublished,
    int? Seconds,
    int? Minutes,
    int? Hours);