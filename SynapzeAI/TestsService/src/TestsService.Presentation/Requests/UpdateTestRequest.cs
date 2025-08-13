using TestsService.Application.Models.Dtos;

namespace TestsService.Presentation.Requests;

public record UpdateTestRequest(
    string TestName,
    string Theme,
    bool WithAI,
    int? Seconds,
    int? Minutes,
    bool? IsPrivate,
    IEnumerable<string>? UsersNamesAreAllowed,
    IEnumerable<string>? UsersEmailsAreAllowed,
    IEnumerable<CreateTaskDto>? TasksToCreate,
    IEnumerable<UpdateTaskDto>? TasksToUpdate,
    IEnumerable<Guid>? TaskIdsToDelete);