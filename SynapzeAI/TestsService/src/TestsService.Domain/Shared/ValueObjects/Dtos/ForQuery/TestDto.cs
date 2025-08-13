using System.ComponentModel.DataAnnotations.Schema;

namespace TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

public record TestDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    
    public string TestName { get; set; }
    public string Theme { get; set; }
    public bool WithAI { get; set; }

    public LimitTimeDto? LimitTime { get; set; }
    
    public PrivacySettingsDto PrivacySettings { get; set; }
    
    [NotMapped]
    public TaskDto[] Tasks { get; set; }
}