using FluentValidation;
using TestsService.Application.Validation;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Dtos;

namespace TestsService.Application.Validators
{
    public class UploadFileDtoValidator : AbstractValidator<UploadFileDto>
    {
        public UploadFileDtoValidator() 
        {
            RuleFor(n => n.FileName).Must(n => string.IsNullOrWhiteSpace(n) == false)
                .WithError(Errors.General.ValueIsRequired("File name"));

            RuleFor(n => n.ContentType).Must(n => n.Length < 10000000)
                .WithError(Errors.General.ValueIsInvalid("Content type"));
        }
    }
}
