using System.Collections;
using TestsService.Application.Models.Dtos;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

namespace TestsService.Presentation.Requests;

public record CreateTestRequest(
    string TestName,
    string Theme,
    bool WithAI,
    int? Seconds,
    int? Minutes,
    IEnumerable<CreateTaskDto> Tasks);