using Application.Abstractions;
using Core;
using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using TestsService.Application.Abstractions;
using TestsService.Application.Models.Dtos;
using TestsService.Application.Providers;
using TestsService.Application.Repositories;
using TestsService.Domain;
using TestsService.Domain.Shared.ValueObjects.Id;
using TestsService.Domain.Shared.ValueObjects.Infos;

namespace TestsService.Application.SolvingHistories.Commands.ExplainTasks;

public class ExplainTasksHandler : ICommandHandler<Guid, Result<IEnumerable<UpdateTaskHistoryDto>, ErrorList>>
{
    private readonly ITestRepository _testRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAiProvider _aiProvider;

    public ExplainTasksHandler(ITestRepository testRepository, IUnitOfWork unitOfWork, IAiProvider aiProvider)
    {
        _testRepository = testRepository;
        _unitOfWork = unitOfWork;
        _aiProvider = aiProvider;
    }
    
    public async Task<Result<IEnumerable<UpdateTaskHistoryDto>, ErrorList>> Handle(
        Guid solvingHistoryId, 
        CancellationToken cancellationToken = default)
    {
        var solvingHistoryResult = await _testRepository
            .GetSolvingHistoryById(SolvingHistoryId.Create(solvingHistoryId), cancellationToken);

        if (solvingHistoryResult.IsFailure)
            return (ErrorList)solvingHistoryResult.Error;
        
        var solvingHistory = solvingHistoryResult.Value;

        var tasksToExplain = solvingHistory.TaskHistories
            .Where(t => t.RightAnswer is null || t.UserAnswer.ToLower() != t.RightAnswer.ToLower())
            .ToArray();
        
        if (tasksToExplain.Length < 1)
            return (ErrorList)Error.Failure(
                "explain.tasks.failure", "Solving history does not contain tasks to explain");

        var taskHistoriesInfo = tasksToExplain.Select(t => new TaskHistoryInfo()
        {
            TaskName = t.TaskName,
            TaskMessage = t.TaskMessage,
            RightAnswer = t.RightAnswer,
            UserAnswer = t.UserAnswer,
            Answers = t.Answers?.ToArray(),
            SerialNumber = t.SerialNumber,
        })
        .ToArray();
        
        var request = GeneratePrompt(taskHistoriesInfo);
        var aiResponse = await _aiProvider.SendToAi(request, null, cancellationToken);
        
        if (aiResponse.IsFailure)
            return (ErrorList)aiResponse.Error;
        
        var explainedTasksResult = JsonParser.TryGetTypeFromString<UpdateTaskHistoryDto[]>(aiResponse.Value, true);
        
        if  (explainedTasksResult.IsFailure)
            return (ErrorList)explainedTasksResult.Error;
        
        var explainedTasks = explainedTasksResult.Value;  
        
        var taskHistoriesBySerialNumber = solvingHistory.TaskHistories
            .ToDictionary(sn => sn.SerialNumber);
        
        foreach (var task in explainedTasks)
        {
            if (taskHistoriesBySerialNumber.TryGetValue(task.SerialNumber, out var taskHistory))
            {
                var updateMessageResult = taskHistory.UpdateMessage(task.Message);

                if (updateMessageResult.IsFailure)
                    return (ErrorList)updateMessageResult.Error;

                taskHistory.UpdatePoints(task.Points);
            }
        }
        
        await _unitOfWork.SaveChanges(cancellationToken);

        return explainedTasks;
    }
    
    private string GeneratePrompt(TaskHistoryInfo[] taskHistories)
    {
        var requestJson = JsonConvert.SerializeObject(taskHistories);
        var responseJson = JsonConvert.SerializeObject(new UpdateTaskHistoryDto(1, "YourText", 100));

        var request = $"История решения задач представлена в формате json: {requestJson}. " +
                      "Цель: проанализировать каждую задачу и ответ пользователя к ней, объяснить " +
                      "почему тот или иной ответ верный / неверный, " +
                      $"вернуть сообщение в формате json: ${responseJson} " +
                      "и поставить оценку от 0 до 100 каждому ответу";
        
        return request;
    }
}