using System.ComponentModel.DataAnnotations.Schema;

namespace TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

public record TaskDto
{
    public Guid Id { get; set; }
    public Guid TestId { get; set; }
    
    public int SerialNumber { get; set; }
    public string TaskName { get; set; }
    public string TaskMessage { get; set; }
    public string? RightAnswer { get; set; }
    
    [NotMapped]
    public TaskStatisticDto? TaskStatistic { get; set; }
    
    public IEnumerable<TaskStatisticDto> TaskStatistics { get; set; }
    
    public string[]?  Answers { get; set; }
}