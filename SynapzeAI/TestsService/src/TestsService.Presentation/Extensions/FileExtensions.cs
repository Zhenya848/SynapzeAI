using Core;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;

namespace TestsService.Presentation.Extensions;

public static class FileExtensions
{
    public static async Task<Result<string, Error>> ConvertToBase64Async(this IFormFile file)
    {
        if (file.Length == 0)
            return Errors.General.ValueIsRequired("file");

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        byte[] fileBytes = memoryStream.ToArray();
        
        string base64String = Convert.ToBase64String(fileBytes);
        
        string mimeType = GetMimeType(file.FileName);
        
        return $"data:{mimeType};base64,{base64String}";
    }

    private static string GetMimeType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".pdf" => "application/pdf",
            ".txt" => "text/plain",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            _ => "application/octet-stream"
        };
    }
}