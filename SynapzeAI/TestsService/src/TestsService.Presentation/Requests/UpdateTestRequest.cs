namespace TestsService.Presentation.Requests;

public record UpdateTestRequest(
    string TestName,
    string Theme,
    bool IsPublished,
    int? Seconds,
    int? Minutes);