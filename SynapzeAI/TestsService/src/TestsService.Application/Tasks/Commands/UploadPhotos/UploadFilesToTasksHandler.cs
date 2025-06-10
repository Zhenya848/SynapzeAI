using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TestsService.Application.Abstractions;
using TestsService.Application.Files.Commands.Create;
using TestsService.Application.Messaging;
using TestsService.Application.Providers;
using TestsService.Application.Repositories;
using TestsService.Application.Validation;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects;
using TestsService.Domain.Shared.ValueObjects.Id;
using FileInfo = TestsService.Domain.Shared.ValueObjects.FileInfo;

namespace TestsService.Application.Tasks.Commands.UploadPhotos;

public class UploadFilesToTasksHandler : ICommandHandler<UploadFilesToTasksCommand, Result<IEnumerable<string>, ErrorList>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly ITestRepository _testRepository;
    private readonly IFileProvider _fileProvider;
    
    private readonly IValidator<UploadFilesToTasksCommand> _validator;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;

    public UploadFilesToTasksHandler(
        IUnitOfWork unitOfWork,
        ITestRepository testRepository,
        IFileProvider fileProvider,
        IValidator<UploadFilesToTasksCommand> validator,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue)
    {
        _unitOfWork = unitOfWork;
        _testRepository = testRepository;
        _fileProvider = fileProvider;
        _validator = validator;
        _messageQueue = messageQueue;
    }
    
    public async Task<Result<IEnumerable<string>, ErrorList>> Handle(
        UploadFilesToTasksCommand command, 
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator
            .ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ValidationErrorResponse();
        
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        try
        {
            var testResult = await _testRepository
                .GetById(TestId.Create(command.TestId), cancellationToken);

            if (testResult.IsFailure)
                return testResult.Error;

            var tasks = testResult.Value.GetTasksByIds(command.TaskIds);

            if (tasks.Count == 0)
                return (ErrorList)Errors.General.ValueIsInvalid("Tasks not found");
            
            if (tasks.Count != command.Files.Count())
                return (ErrorList)Errors.General.ValueIsInvalid("Number of files does not match");

            List<FileData> files = new List<FileData>();

            for (int i = 0; i < tasks.Count; i++)
            {
                var file = command.Files.ElementAt(i);
                var pathResult = FilePath.Create(file.FileName);

                if (pathResult.IsFailure)
                    return (ErrorList)pathResult.Error;

                tasks[i].UpdateImagePath(pathResult.Value.FullPath);

                var fileData = new FileData(pathResult.Value.FullPath, file.Stream);
                files.Add(fileData);
            }

            await _unitOfWork.SaveChanges(cancellationToken);

            var createFilesCommand = new CreateFilesCommand(files, "photos");
            var uploadResult = await _fileProvider.UploadFiles(createFilesCommand, cancellationToken);

            if (uploadResult.IsFailure)
            {
                List<FileInfo> filesInfo =
                    files.Select(f => new FileInfo("photos", f.FilePath)).ToList();
                
                await _messageQueue.WriteAsync(filesInfo, cancellationToken);
                
                return (ErrorList)uploadResult.Error;
            }

            transaction.Commit();

            var result = files.Select(p => p.FilePath);

            return Result.Success<IEnumerable<string>, ErrorList>(result);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            
            return (ErrorList)Error.Failure("Can not to upload files to pet. " + ex.Message, "upload.files.failure");
        }
    }
}