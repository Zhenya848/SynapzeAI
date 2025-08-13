namespace FileService.Web.Abstractions;

public interface IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints);
}