using CSharpFunctionalExtensions;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Id;
using TestsService.Domain.ValueObjects;

namespace TestsService.Domain;

public class Test : SoftDeletableEntity<TestId>
{
    public Guid UserId { get; private set; }
    
    public string TestName { get; private set; }
    public string Theme { get; private set; }
    public bool IsPublished { get; private set; }
    
    public LimitTime? LimitTime { get; private set; }
    
    private List<Task> _tasks  = new List<Task>();
    public IReadOnlyList<Task> Tasks => _tasks;

    private Test(TestId id) : base(id)
    {
        
    }
    
    private Test(
        TestId id, 
        Guid userId,
        string testName, 
        string theme,
        bool isPublished,
        LimitTime? limitTime = null,
        IEnumerable<Task>? tasks = null) : base(id)
    {
        TestName = testName;
        Theme = theme;
        UserId = userId;
        IsPublished = isPublished;
        LimitTime = limitTime;
        _tasks = tasks?.ToList() ?? [];
    }

    public static Result<Test, Error> Create(
        TestId id, 
        Guid userId,
        string testName, 
        string theme,
        bool isPublished,
        LimitTime? limitTime = null,
        IEnumerable<Task>? tasks = null)
    {
        if (string.IsNullOrWhiteSpace(testName))
            return Errors.General.ValueIsRequired(nameof(TestName));
        
        if (string.IsNullOrWhiteSpace(theme))
            return Errors.General.ValueIsRequired(nameof(Theme));
        
        return new Test(id, userId, testName, theme, isPublished, limitTime, tasks);
    }

    public void AddTasks(IEnumerable<Task> tasks) =>
        _tasks.AddRange(tasks);

    public List<Task> GetTasksByIds(IEnumerable<Guid> TaskIds)
    {
        var result = new List<Task>();

        foreach (var taskId in TaskIds)
        {
            var task = _tasks.FirstOrDefault(i => i.Id == taskId);
            
            if (task != null)
                result.Add(task);
        }
        
        return result;
    }
    
    public void UpdateInfo(
        string testName,
        string theme,
        bool isPublished,
        LimitTime? limitTime)
    {
        TestName = testName;
        Theme = theme;
        IsPublished = isPublished;
        LimitTime = limitTime;
    }

    public override void Delete()
    {
        base.Delete();

        foreach (var task in Tasks)
            task.Delete();
    }

    public override void Restore()
    {
        base.Restore();

        foreach (var task in Tasks)
            task.Restore();
    }
}