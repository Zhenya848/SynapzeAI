using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TestsService.Application.Abstractions;
using TestsService.Application.Extensions;
using TestsService.Application.Providers;
using TestsService.Application.Repositories;
using TestsService.Domain;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;
using TestsService.Domain.Shared.ValueObjects.Id;
using TestsService.Domain.ValueObjects;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Task = TestsService.Domain.Task;

namespace TestsService.Application.Tests.Commands.CreateWithAI;

public class CreateTestWithAIHandler : ICommandHandler<CreateTestWithAICommand, Result<Guid, ErrorList>>
{
    private readonly ITestRepository _testRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAIProvider _aiProvider;

    public CreateTestWithAIHandler(
        IUnitOfWork unitOfWork,
        ITestRepository testRepository,
        IAIProvider aiProvider)
    {
        _testRepository = testRepository;
        _unitOfWork = unitOfWork;
        _aiProvider = aiProvider;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        CreateTestWithAICommand command, 
        CancellationToken cancellationToken = default)
    {
        if (command.TasksCount is not null && command.TasksCount < 1)
            return (ErrorList)Errors.General.ValueIsInvalid(nameof(command.TasksCount));
        
        if (string.IsNullOrWhiteSpace(command.Theme))
            return (ErrorList)Errors.General.ValueIsInvalid(nameof(command.Theme));
        
        string testFormat = "{\n  \"testName\": \"string\",\n  \"theme\": \"string\",\n  " +
                            "\"withAI\": true,\n  \"limitTime\": {\n    \"seconds\": number,\n    " +
                            "\"minutes\": number\n  },\n  " +
                            "\"tasks\": [\n    {\n      \"taskName\": \"string\",\n      " +
                            "\"taskMessage\": \"string\",\n      \"rightAnswer\": \"string\",\n      " +
                            "\"answers\": [\n        \"string\"\n      ]\n    }\n  ]\n}";

        string userRequest = $"Сгенерируй тест по формату json: {testFormat}. " +
                             $"Тема теста: {command.Theme}, " +
                             $"Сложность: {(command.Difficulty is null ? "на выбор" : $"{command.Difficulty}%")}, " +
                             $"поле limitTime: {(command is { Seconds: not null, Minutes: not null } || command.IsTimeLimited
                                 ? "добавлять" : "не добавлять")}, " +
                             $"{(command is { Seconds: not null, Minutes: not null }
                                 ? $"секунд: {command.Seconds}, минут: {command.Minutes}, " : "")}" +
                             $"количество задач: {(command.TasksCount is not null
                                 ? command.TasksCount.ToString() : "не ограничено")}, " +
                             $"Процент открытых задач: {command.PercentOfOpenTasks}. " +
                             $"Задачи не обязательно должны иметь answers или rightAnswer, если это задачи открытого типа";
        
        var response = await _aiProvider.GenerateContent(userRequest, cancellationToken);

        if (response.IsFailure)
            return response.Error;
        
        var testDataResult = JsonExtensions.TryGetTypeFromString<TestDto>(response.Value);

        if (testDataResult.IsFailure)
            return (ErrorList)testDataResult.Error;
        
        var testData = testDataResult.Value;

        var tasks = testData.Tasks.Select(t => Task.Create(
            TaskId.AddNewId(), t.TaskName, t.TaskMessage, t.RightAnswer, t.Answers));
        
        if (tasks.Any(t => t.IsFailure))
            return (ErrorList)tasks.Select(t => t.Error).ToList();
        
        Result<LimitTime, Error> limitTime = default;
    
        if (testData.LimitTime is not null)
        {
            limitTime = LimitTime.Create(testData.LimitTime.Seconds, testData.LimitTime.Minutes);

            if (limitTime.IsFailure)
                return (ErrorList)limitTime.Error;
        }

        var test = Test.Create(
            TestId.AddNewId(),
            command.UserId,
            testData.TestName,
            testData.Theme,
            true,
            limitTime.IsSuccess ? limitTime.Value : null,
            tasks.Select(t => t.Value));
        
        if (test.IsFailure)
            return (ErrorList)test.Error;
        
        var result = _testRepository.Add(test.Value);
    
        await _unitOfWork.SaveChanges(cancellationToken);

        return result;
    }
    
    public record GenerateContentRequest(string UserRequest);
}
