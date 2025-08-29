namespace TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

public record SavedTestDto
{
    public Guid Id { get; set; }
    
    public Guid TestId { get; set; }
    public Guid UserId { get; set; }
}