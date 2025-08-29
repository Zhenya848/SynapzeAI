namespace TestsService.Domain.Shared;

public class Errors
{
    public class General
    {
        public static Error ValueIsInvalid(string? name = null) =>
            Error.Validation("value.is.invalid", $"Значение для: {(name != null ? name : "value")} невалидно");

        public static Error NotFound(Guid? id = null) =>
            Error.NotFound("record.not.found", $"объект с id: {(id != null ? " for id: " + id : "")} не найден");

        public static Error Failure(string? name = null) =>
            Error.Failure("failure", $"{(name != null ? name : "value")} is failure");

        public static Error ValueIsRequired(string? name = null) =>
            Error.Conflict("value.is.required", $"Ожидается значение для: {(name != null ? name : "value")}");
    }
    
    public static class Token
    {
        public static Error InvalidToken()
        {
            return Error.Validation("token.is.invalid", "Your token is invalid");
        }
    }
}