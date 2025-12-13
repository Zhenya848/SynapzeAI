using TestsService.Domain.Shared.ValueObjects.Dtos;

namespace TestsService.Domain.Shared.ValueObjects.Infos;

public record TestInfo
{
    public string TestName { get; set; }
    public string Theme { get; set; }
    public LimitTimeDto LimitTime { get; set; }
    public TaskInfo[] Tasks { get; set; }
}