using TestsService.Domain.Shared.ValueObjects.Dtos;

namespace TestsService.Application.Tasks.Commands.UploadPhotos;

public record UploadFilesToTasksCommand(
    Guid TestId,
    IEnumerable<Guid> TaskIds,
    IEnumerable<UploadFileDto> Files);