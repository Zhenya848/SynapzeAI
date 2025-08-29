using System.ComponentModel.DataAnnotations.Schema;

namespace TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

public record TestDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public string UniqueUserName { get; set; }
    
    public string TestName { get; set; }
    public string Theme { get; set; }
    
    public bool IsPublished { get; set; }
    
    [NotMapped]
    public bool IsSaved { get; set; }

    public LimitTimeDto? LimitTime { get; set; }
    
    public IEnumerable<TaskDto> Tasks { get; set; }
    public IEnumerable<SavedTestDto> SavedTests { get; set; }
}