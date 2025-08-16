using CSharpFunctionalExtensions;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;
using TestsService.Domain.Shared.ValueObjects.Id;
using TestsService.Domain.ValueObjects;

namespace TestsService.Domain;

public class Test : Shared.Entity<TestId>
{
    public Guid UserId { get; private set; }
    
    public string UniqueUserName { get; private set; }
    
    public string TestName { get; private set; }
    public string Theme { get; private set; }
    
    public LimitTime? LimitTime { get; private set; }
    
    public bool IsPublished { get; private set; }
    
    private List<Task> _tasks  = new List<Task>();
    public IReadOnlyList<Task> Tasks => _tasks;
    
    private List<SolvingHistory> _solvingHistories = new List<SolvingHistory>();
    public IReadOnlyList<SolvingHistory> SolvingHistories => _solvingHistories;

    private Test(TestId id) : base(id)
    {
        
    }
    
    private Test(
        TestId id, 
        Guid userId,
        string uniqueUserName,
        string testName, 
        string theme,
        bool isPublished,
        LimitTime? limitTime = null,
        IEnumerable<Task>? tasks = null) : base(id)
    {
        UniqueUserName = uniqueUserName;
        TestName = testName;
        Theme = theme;
        UserId = userId;
        IsPublished = isPublished;
        LimitTime = limitTime;
        
        if (tasks is not null)
            AddTasks(tasks);
    }

    public static Result<Test, Error> Create(
        TestId id, 
        Guid userId,
        string uniqueUserName,
        string testName, 
        string theme,
        bool isPublished,
        LimitTime? limitTime = null,
        IEnumerable<Task>? tasks = null)
    {
        if (string.IsNullOrWhiteSpace(uniqueUserName))
            return Errors.General.ValueIsRequired(nameof(UniqueUserName));
        
        if (string.IsNullOrWhiteSpace(testName))
            return Errors.General.ValueIsRequired(nameof(TestName));
        
        if (string.IsNullOrWhiteSpace(theme))
            return Errors.General.ValueIsRequired(nameof(Theme));
        
        return new Test(id, userId, uniqueUserName, testName, theme, isPublished, limitTime, tasks);
    }

    public void AddTasks(IEnumerable<Task> tasks)
    {
        tasks.ToList().ForEach(t =>
        {
            t.SetSerialNumber(_tasks.Count + 1);
            _tasks.Add(t);
        });
    }

    public void UpdateTasks(IEnumerable<Task> tasks)
    {
        var taskIds = tasks.Select(t => t.Id);

        foreach (var task in _tasks.Where(t => taskIds.Contains(t.Id)))
        {
            var newTask = tasks.First(i => i.Id == task.Id);

            task.UpdateInfo(
                newTask.TaskName,
                newTask.TaskMessage,
                newTask.RightAnswer,
                newTask.Answers);
        }
    }

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
    
    public UnitResult<Error> UpdateInfo(
        string uniqueUserName,
        string testName,
        string theme,
        bool isPublished,
        LimitTime? limitTime)
    {
        if (string.IsNullOrWhiteSpace(uniqueUserName))
            return Errors.General.ValueIsRequired(nameof(UniqueUserName));
        
        if (string.IsNullOrWhiteSpace(testName))
            return Errors.General.ValueIsRequired(nameof(TestName));
        
        if (string.IsNullOrWhiteSpace(theme))
            return Errors.General.ValueIsRequired(nameof(Theme));
        
        TestName = testName;
        Theme = theme;
        IsPublished = isPublished;
        LimitTime = limitTime;

        return Result.Success<Error>();
    }
    
    public void AddSolvingHistory(SolvingHistory solvingHistory) =>
        _solvingHistories.Add(solvingHistory);

    public void UpdateSolvingHistory(SolvingHistory solvingHistory)
    {
        _solvingHistories[_solvingHistories.FindIndex(s => s.Id == solvingHistory.Id)]
            .UpdateInfo(
                solvingHistory.UniqueUserName,
                solvingHistory.UserEmail,
                solvingHistory.TaskHistories,
                solvingHistory.SolvingDate,
                solvingHistory.SolvingTimeSeconds);
    }
}