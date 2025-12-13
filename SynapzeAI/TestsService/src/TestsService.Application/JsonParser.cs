using Core;
using CSharpFunctionalExtensions;
using Newtonsoft.Json;

namespace TestsService.Application;

public class JsonParser
{
    public static Result<T, Error> TryGetTypeFromString<T>(string value, bool isCollection)
    {
        try
        {
            var startChar = isCollection ? '[' : '{';
            var endChar = isCollection ? ']' : '}';

            var startIndex = value.IndexOf(startChar);
            var endIndex = value.LastIndexOf(endChar);

            if (startIndex == -1 || endIndex == -1 || startIndex >= endIndex)
                return default;

            var json = value.Substring(startIndex, endIndex - startIndex + 1);
            
            return JsonConvert.DeserializeObject<T>(json)!;
        }
        catch (Exception e)
        {
            return Error.Failure("json.parse.error", $"Cannot parse json to value type: {e.Message}");
        }
    }
}