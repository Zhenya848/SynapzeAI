using TestsService.Domain.Shared.ValueObjects;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

namespace TestsService.Application.Models.Dtos;

public record GlobalTestDto(
    TestDto Test,
    UserInfo User);