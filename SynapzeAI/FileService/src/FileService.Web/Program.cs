using Amazon.S3;
using FileService.Web;
using FileService.Web.Options;
using Minio;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddEndpoints();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MinioOptions>(
    builder.Configuration.GetSection(MinioOptions.MINIO));

builder.Services.AddMinio(options =>
{
    MinioOptions minioOptions = builder.Configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>()
                                ?? throw new ApplicationException("Minio options is missing.");

    options.WithEndpoint(minioOptions.Endpoint);
    options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);

    options.WithSSL(minioOptions.WithSsl);
});

builder.Services.AddSingleton<IAmazonS3>(_ =>
{
    var config = new AmazonS3Config()
    {
        ServiceURL = "http://localhost:9000",
        ForcePathStyle = true
    };

    return new AmazonS3Client("minioadmin", "minioadmin", config);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(config =>
{
    config.WithOrigins("http://localhost:5173")
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod();
});

app.UseHttpsRedirection();
app.MapEndpoints();
app.Run();