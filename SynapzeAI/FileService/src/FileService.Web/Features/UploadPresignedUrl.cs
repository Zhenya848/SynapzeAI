using Amazon.S3;
using Amazon.S3.Model;
using FileService.Web.Abstractions;

namespace FileService.Web.Features;

public class UploadPresignedUrl
{
    private record UploadPresignedUrlRequest(
        string FileName, 
        string ContentType,
        long Size);
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("files/presigned", Handler);
        }
    }

    private static async Task<IResult> Handler(
        UploadPresignedUrlRequest request,
        IAmazonS3 s3Client,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var key = Guid.NewGuid();

            var args = new GetPreSignedUrlRequest
            {
                BucketName = "bucket",
                Key = $"photos/{key}",
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddMinutes(2),
                ContentType = request.ContentType,
                Protocol = Protocol.HTTP,
                Metadata = { ["file_name"] = request.FileName }
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