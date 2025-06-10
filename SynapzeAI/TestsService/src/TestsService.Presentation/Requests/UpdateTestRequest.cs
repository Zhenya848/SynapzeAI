namespace TestsService.Presentation.Requests;

public record UpdateTestRequest(
    string TestName,
    bool IsPublished,
    int? Seconds,
    int? Minutes,
    int? Hours);