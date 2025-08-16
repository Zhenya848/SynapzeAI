using TestsService.Application.Models.Dtos;

namespace TestsService.Presentation.Requests;

public record UpdateTestRequest(
    string UniqueUserName,
    string TestName,
    string Theme,
    bool IsPublished,
    int? Seconds,
    int? Minutes,
    IEnumerable<CreateTaskDto>? TasksToCreate,
    IEnumerable<UpdateTaskDto>? TasksToUpdate,
    IEnumerable<Guid>? TaskIdsToDelete);