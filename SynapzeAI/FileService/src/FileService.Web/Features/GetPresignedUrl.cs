using Amazon.S3;
using Amazon.S3.Model;
using FileService.Web.Abstractions;

namespace FileService.Web.Features;

public class GetPresignedUrl
{
    private record GetPresignedUrlRequest(
        string FileName, 
        string ContentType,
        long Size);
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("files/{key:guid}/presigned", Handler);
        }
    }

    private static async Task<IResult> Handler(
        Guid key,
        IAmazonS3 s3Client,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var args = new GetPreSignedUrlRequest
            {
                BucketName = "bucket",
                Key = $"photos/{key}",
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddHours(24),
                Protocol = Protocol.HTTP
            };

            var url = await s3Client.GetPreSignedURLAsync(args);

            return Results.Ok(new { key, url });
        }
        catch (AmazonS3Exception e)
        {
            return Results.BadRequest($"S3 Exception: {e.Message}");
        }
    }
}