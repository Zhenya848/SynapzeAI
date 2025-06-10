namespace TestsService.Domain.Shared.ValueObjects.Dtos
{
    public record UploadFileDto(string FileName, string ContentType, Stream Stream);
}
