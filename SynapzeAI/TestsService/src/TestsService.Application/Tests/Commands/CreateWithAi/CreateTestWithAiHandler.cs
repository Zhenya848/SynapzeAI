using Application.Abstractions;
using Core;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using TestsService.Application.Abstractions;
using TestsService.Application.Providers;
using TestsService.Application.Repositories;
using TestsService.Domain;
using TestsService.Domain.Shared.ValueObjects.Id;
using TestsService.Domain.Shared.ValueObjects.Infos;
using TestsService.Domain.ValueObjects;
using Task = TestsService.Domain.Task;

namespace TestsService.Application.Tests.Commands.CreateWithAi;

public class CreateTestWithAiHandler : ICommandHandler<CreateTestWithAiCommand, Result<Guid, ErrorList>>
{
    private readonly ITestRepository _testRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGreeterService _greeterService;
    private readonly IAiProvider _aiProvider;
    private readonly ILogger<CreateTestWithAiHandler> _logger;

    public CreateTestWithAiHandler(
        ITestRepository testRepository, 
        IUnitOfWork unitOfWork, 
        IGreeterService greeterService, 
        IAiProvider aiProvider,
        ILogger<CreateTestWithAiHandler> logger)
    {
        _testRepository = testRepository;
        _unitOfWork = unitOfWork;
        _greeterService = greeterService;
        _aiProvider = aiProvider;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateTestWithAiCommand command, 
        CancellationToken cancellationToken = default)
    {
        var isSubtractFromTrialBalance = command.Base64File is null;
        
        var subtractTokenFromBalance = await _greeterService.SubtractTokenFromBalance(
            command.UserId, 
            isSubtractFromTrialBalance, 
            cancellationToken);

        if (subtractTokenFromBalance.IsFailure)
            return subtractTokenFromBalance.Error;

        isSubtractFromTrialBalance = subtractTokenFromBalance.Value == nameof(BalanceType.Trial);

        try
        {
            var request = GeneratePrompt(command);
            
            var aiResponse = await _aiProvider
                .SendToAi(request, command.Base64File, cancellationToken);

            if (aiResponse.IsFailure)
                return await ReturnError(
                    aiResponse.Error, 
                    command.UserId, 
                    isSubtractFromTrialBalance, 
                    cancellationToken);

            var testInfoResult = JsonParser
                .TryGetTypeFromString<TestInfo>(aiResponse.Value, false);
            
            if (testInfoResult.IsFailure)
                return await ReturnError(
                    testInfoResult.Error, 
                    command.UserId, 
                    isSubtractFromTrialBalance, 
                    cancellationToken);
            
            var testInfo = testInfoResult.Value;
            
            var tasks = testInfo.Tasks
                .Select(t => Task.Create(
                    TaskId.AddNewId(),
                    t.TaskName,
                    t.TaskMessage,
                    t.RightAnswer,
                    t.Answers))
                .ToList();
            
            if (tasks.Any(t => t.IsFailure))
                return await ReturnError(
                    tasks.Select(t => t.Error).ToList(), 
                    command.UserId, 
                    isSubtractFromTrialBalance,
                    cancellationToken);
            
            var limitTimeResult = LimitTime.Create(
                testInfo.LimitTime.Seconds, 
                testInfo.LimitTime.Minutes);
            
            if (limitTimeResult.IsFailure)
                return await ReturnError(
                    limitTimeResult.Error, 
                    command.UserId, 
                    isSubtractFromTrialBalance, 
                    cancellationToken);
            
            var test = Test.Create(
                TestId.AddNewId(), 
                command.UserId,
                command.UniqueUserName,
                testInfo.TestName, 
                testInfo.Theme,
                false,
                limitTimeResult.Value,
                tasks.Select(t => t.Value));
            
            if (test.IsFailure)
                return await ReturnError(
                    test.Error, 
                    command.UserId, 
                    isSubtractFromTrialBalance, 
                    cancellationToken);
            
            var result = _testRepository.Add(test.Value);
            
            await _unitOfWork.SaveChanges(cancellationToken);

            return result;
        }
        catch (Exception)
        {
            return await ReturnError(
                Errors.General.Failure(), 
                command.UserId, 
                isSubtractFromTrialBalance,
                cancellationToken);
        }
    }

    private string GeneratePrompt(in CreateTestWithAiCommand command)
    {
        var testFormat = @"{
          ""testName"": ""string"",
          ""theme"": ""string"",
          ""limitTime"": {
            ""seconds"": ""int"",
            ""minutes"": ""int""
          },
          ""tasks"": [
            {
              ""taskName"": ""string"",
              ""taskMessage"": ""string"",
              ""rightAnswer"": ""string"",
              ""answers"": [
                ""string""
              ]
            }
          ]
        }";
        
        var seconds = command.Seconds ?? 0;
        var minutes = command.Minutes ?? 0;
        
        var request = $@"Сгенерируй тест по формату json: {testFormat}. 
            
            Тема теста: {command.TestTheme}, Сложность: {command.Difficulty},
            {(seconds + minutes * 60 >= 10
                ? $"секунд: {command.Seconds ?? 0}, минут: {command.Minutes ?? 0}, " : "время на решение выбери сам, ")}
            количество задач для теста: {(command.TasksCount is > 0 ? command.TasksCount : "максимум 20")}. 
            Процент открытых задач по отношению к задачам на выбор ответа: {command.PercentOfOpenTasks}. 
            Задачи не должны иметь answers и rightAnswer, если это задачи открытого типа. 
            Спецсимволы не используй. Ограничения по длине: taskName: 50 символов, 
            taskMessage: 1000 символов, rightAnswer: 50 символов.";
        
        return request;
    }

    private async Task<ErrorList> ReturnError(
        ErrorList errorList, 
        Guid userId, 
        bool isTrialBalance,
        CancellationToken cancellationToken)
    {
        var addTokenToBalance = await _greeterService
            .AddTokenToBalance(userId, isTrialBalance, cancellationToken);

        if (addTokenToBalance.IsFailure)
        {
            var errors = string.Join(", ", addTokenToBalance.Error
                .Select(m => $"code: {m.Code} message: {m.Message}"));
            
            _logger.LogCritical("Cannot return token to user {userId}. {errors}", userId, errors);
        }
        
        return errorList;
    }
}