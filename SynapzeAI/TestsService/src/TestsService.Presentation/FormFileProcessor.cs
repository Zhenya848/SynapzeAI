using Microsoft.AspNetCore.Http;
using TestsService.Domain.Shared.ValueObjects.Dtos;

namespace TestsService.Presentation;

public class FormFileProcessor : IAsyncDisposable
{
    private readonly List<UploadFileDto> _filesDto = new List<UploadFileDto>();

    public List<UploadFileDto> StartProcess(IFormFileCollection files)
    {
        foreach (var file in files)
        {
            var stream = file.OpenReadStream();
            _filesDto.Add(new UploadFileDto(file.FileName, file.ContentType, stream));
        }

        return _filesDto;
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var file in _filesDto)
            await file.Stream.DisposeAsync();
    }
}