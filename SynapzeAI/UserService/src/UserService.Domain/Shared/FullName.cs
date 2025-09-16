using Core;
using CSharpFunctionalExtensions;

namespace UserService.Domain.Shared
{
    public record FullName
    {
        public string FirstName { get; } = default!;
        public string LastName { get; } = default!;
        public string Patronymic { get; } = default!;

        private FullName(string firstName, string lastName, string patronymic)
        {
            FirstName = firstName;
            LastName = lastName;
            Patronymic = patronymic;
        }

        public static Result<FullName, Error> Create(string firstName, string lastName, string patronymic)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                return Errors.General.ValueIsInvalid("first name is null or white space! first name");

            if (string.IsNullOrWhiteSpace(lastName))
                return Errors.General.ValueIsInvalid("last name is null or white space! last name");

            return new FullName(firstName, lastName, patronymic);
        }
    }
}
