using System.Collections;
using TestsService.Application.Models.Dtos;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

namespace TestsService.Presentation.Requests;

public record CreateTestRequest(
    string UniqueUserName,
    string TestName,
    string Theme,
    bool IsPublished,
    int? Seconds,
    int? Minutes,
    IEnumerable<CreateTaskDto> Tasks);