using FluentValidation;
using TestsService.Application.Tasks.Commands.UploadPhotos;
using TestsService.Application.Validation;
using TestsService.Domain.Shared;

namespace TestsService.Application.Validators
{
    public class UploadFilesToTasksCommandValidator : AbstractValidator<UploadFilesToTasksCommand>
    {
        public UploadFilesToTasksCommandValidator()
        {
            RuleFor(i => i.TaskIds).NotEmpty().WithError(Errors.General.ValueIsRequired("Task ids"));

            RuleFor(f => f.Files).NotEmpty().WithError(Errors.General.ValueIsRequired("File list"));
            RuleForEach(f => f.Files).SetValidator(new UploadFileDtoValidator());
        }
    }
}
