using System.Collections;
using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

namespace TestsService.Application.Extensions;

public static class JsonExtensions
{
    public static Result<T, Error> TryGetTypeFromString<T>(string value)
    {
        try
        {   
            var isCollection = typeof(T) != typeof(string) && typeof(T).GetInterfaces().Contains(typeof(IEnumerable));
            
            int startIndex = value.IndexOf(isCollection ? '[' : '{');
            int endIndex = value.LastIndexOf(isCollection ? ']' : '}');

            string json = value.Substring(startIndex, endIndex - startIndex + 1);

            var settings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var result = JsonConvert.DeserializeObject<T>(json, settings);

            return result;
        }
        catch (Exception e)
        {
            return Error.Deserialize(e.Message);
        }
    }
}