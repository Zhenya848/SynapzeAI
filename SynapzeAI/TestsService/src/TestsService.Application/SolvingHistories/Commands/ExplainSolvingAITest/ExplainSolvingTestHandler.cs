using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using TestsService.Application.Abstractions;
using TestsService.Application.Extensions;
using TestsService.Application.Models.Dtos;
using TestsService.Application.Providers;
using TestsService.Application.Repositories;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Id;

namespace TestsService.Application.SolvingHistories.Commands.ExplainSolvingAITest;

public class ExplainSolvingTestHandler 
    : ICommandHandler<ExplainSolvingTestCommand, Result<IEnumerable<AIMessageForTask>, ErrorList>>
{
    private readonly IAIProvider _aiProvider;
    private readonly ITestRepository _testRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ExplainSolvingTestHandler(
        IAIProvider aiProvider, 
        ITestRepository testRepository, 
        IUnitOfWork unitOfWork)
    {
        _aiProvider = aiProvider;
        _testRepository = testRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<IEnumerable<AIMessageForTask>, ErrorList>> Handle(
        ExplainSolvingTestCommand command, 
        CancellationToken cancellationToken = default)
    {
        var testResult = await _testRepository.GetById(TestId.Create(command.TestId), cancellationToken);

        if (testResult.IsFailure)
            return testResult.Error;
        
        var test = testResult.Value;
        var solvingHistory = test.SolvingHistories.FirstOrDefault(sh => sh.Id == command.SolvingHistoryId);
        
        if (solvingHistory is null)
            return (ErrorList)Errors.General.NotFound(command.SolvingHistoryId);

        var taskHistoriesToExplain = solvingHistory.TaskHistories
            .Where(th => th.RightAnswer is null || th.UserAnswer.ToLower() != th.RightAnswer.ToLower());

        var tasksHistoryJson = JsonConvert.SerializeObject(taskHistoriesToExplain);
        var aiMessageForTaskJsonFormat = JsonConvert.SerializeObject(new AIMessageForTask[] { new (1, "Your text") });

        var userRequest = $"История решения задач представлена в формате json: {tasksHistoryJson}. " +
                          $"Цель: проанализировать каждую задачу и ответ пользователя к ней " +
                          $"и вернуть сообщение в формате json: {aiMessageForTaskJsonFormat}. ";
        
        var response = await _aiProvider.GenerateContent(userRequest, cancellationToken);
        
        if (response.IsFailure)
            return response.Error;
        
        var aiMessagesDataResult = JsonExtensions.TryGetTypeFromString<AIMessageForTask[]>(response.Value);
        
        if (aiMessagesDataResult.IsFailure)
            return (ErrorList)aiMessagesDataResult.Error;
        
        var aiMessagesData = aiMessagesDataResult.Value;

        solvingHistory.TaskHistories.ForEach(th =>
            th.UpdateAIMessage(aiMessagesData.FirstOrDefault(v => 
                v.TaskSerialNumber == th.SerialNumber)?.AIMessage ?? th.MessageAI));
        
        test.UpdateSolvingHistory(solvingHistory);
        
        await _unitOfWork.SaveChanges(cancellationToken);

        return aiMessagesData;
    }
}