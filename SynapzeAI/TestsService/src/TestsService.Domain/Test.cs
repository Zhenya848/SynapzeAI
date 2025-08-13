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
    public bool WithAI { get; private set; }
    
    public LimitTime? LimitTime { get; private set; }
    
    public PrivacySettings PrivacySettings { get; private set; }
    
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
        string testName, 
        string theme,
        bool withAi,
        LimitTime? limitTime = null,
        PrivacySettings? privacySettings = null,
        IEnumerable<Task>? tasks = null) : base(id)
    {
        TestName = testName;
        Theme = theme;
        UserId = userId;
        WithAI = withAi;
        PrivacySettings = privacySettings ?? PrivacySettings.AddDefault();
        LimitTime = limitTime;
        
        if (tasks is not null)
            AddTasks(tasks);
    }

    public static Result<Test, Error> Create(
        TestId id, 
        Guid userId,
        string testName, 
        string theme,
        bool withAI,
        LimitTime? limitTime = null,
        PrivacySettings? privacySettings = null,
        IEnumerable<Task>? tasks = null)
    {
        if (string.IsNullOrWhiteSpace(testName))
            return Errors.General.ValueIsRequired(nameof(TestName));
        
        if (string.IsNullOrWhiteSpace(theme))
            return Errors.General.ValueIsRequired(nameof(Theme));
        
        return new Test(id, userId, testName, theme, withAI, limitTime, privacySettings, tasks);
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
        string testName,
        string theme,
        bool withAI,
        PrivacySettings? privacySettings,
        LimitTime? limitTime)
    {
        if (string.IsNullOrWhiteSpace(testName))
            return Errors.General.ValueIsRequired(nameof(TestName));
        
        if (string.IsNullOrWhiteSpace(theme))
            return Errors.General.ValueIsRequired(nameof(Theme));
        
        TestName = testName;
        Theme = theme;
        WithAI = withAI;
        LimitTime = limitTime;

        if (privacySettings is not null)
            PrivacySettings = privacySettings;

        return Result.Success<Error>();
    }
    
    public void AddSolvingHistory(SolvingHistory solvingHistory) =>
        _solvingHistories.Add(solvingHistory);

    public void UpdateSolvingHistory(SolvingHistory solvingHistory)
    {
        _solvingHistories[_solvingHistories.FindIndex(s => s.Id == solvingHistory.Id)]
            .UpdateInfo(
                solvingHistory.TaskHistories,
                solvingHistory.SolvingDate,
                solvingHistory.SolvingTimeSeconds);
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