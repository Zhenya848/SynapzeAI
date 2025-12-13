using Core;
using CSharpFunctionalExtensions;
using TestsService.Domain.Shared.ValueObjects.Id;
using TestsService.Domain.ValueObjects;

namespace TestsService.Domain;

public class Test : Core.Entity<TestId>
{
    public Guid UserId { get; private set; }
    
    public string UniqueUserName { get; private set; }
    
    public string TestName { get; private set; }
    public string Theme { get; private set; }
    
    public LimitTime? LimitTime { get; private set; }
    
    public bool IsPublished { get; private set; }
    
    private readonly List<Task> _tasks  = [];
    public IReadOnlyList<Task> Tasks => _tasks;
    
    private readonly List<SolvingHistory> _solvingHistories = [];
    public IReadOnlyList<SolvingHistory> SolvingHistories => _solvingHistories;
    
    private readonly List<SavedTest> _savedTests = [];
    public IReadOnlyList<SavedTest> SavedTests => _savedTests;

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
            return Errors.General.ValueIsRequired("user name");
        
        if (string.IsNullOrWhiteSpace(testName))
            return Errors.General.ValueIsRequired("test name");
        
        if (string.IsNullOrWhiteSpace(theme))
            return Errors.General.ValueIsRequired("test theme");
        
        return new Test(id, userId, uniqueUserName, testName, theme, isPublished, limitTime, tasks);
    }

    public void AddTasks(IEnumerable<Task> tasks)
    {
        int startNumber = _tasks.Count + 1;
        
        foreach (var task in tasks)
        {
            task.SetSerialNumber(startNumber++);
            _tasks.Add(task);
        }
    }

    public UnitResult<ErrorList> UpdateTasks(IEnumerable<Task> tasks)
    {
        var tasksById = tasks.ToDictionary(t => t.Id);
        var existingIds = _tasks.ToDictionary(t => t.Id);
        
        var notFoundIds = tasksById.Keys
            .Where(id => existingIds.ContainsKey(id) == false).ToList();

        if (notFoundIds.Count > 0)
            return (ErrorList)notFoundIds
                .Select(id => Errors.General.NotFound(id));

        foreach (var task in _tasks)
        {
            if (tasksById.TryGetValue(task.Id, out var newTask))
            {
                task.UpdateInfo(
                    newTask.TaskName,
                    newTask.TaskMessage,
                    newTask.RightAnswer,
                    newTask.Answers);
            }
        }
        
        return Result.Success<ErrorList>();
    }

    public List<Task> GetTasksByIds(IEnumerable<Guid> taskIds)
    {
        taskIds = taskIds.ToArray();
        
        if (taskIds.Any() == false)
            return [];
    
        var taskIdsSet = new HashSet<Guid>(taskIds);
        
        return _tasks
            .Where(t => taskIdsSet.Contains(t.Id))
            .OrderBy(sn => sn.SerialNumber)
            .ToList();
    }
    
    public UnitResult<Error> UpdateInfo(
        string uniqueUserName,
        string testName,
        string theme,
        bool isPublished,
        LimitTime? limitTime)
    {
        if (string.IsNullOrWhiteSpace(uniqueUserName))
            return Errors.General.ValueIsRequired("user name");
        
        if (string.IsNullOrWhiteSpace(testName))
            return Errors.General.ValueIsRequired("test name");
        
        if (string.IsNullOrWhiteSpace(theme))
            return Errors.General.ValueIsRequired("test theme");
        
        TestName = testName;
        Theme = theme;
        IsPublished = isPublished;
        LimitTime = limitTime;

        return Result.Success<Error>();
    }
    
    public void AddSolvingHistory(SolvingHistory solvingHistory) =>
        _solvingHistories.Add(solvingHistory);
    
    public void AddSavedTest(SavedTest savedTest) =>
        _savedTests.Add(savedTest);
}