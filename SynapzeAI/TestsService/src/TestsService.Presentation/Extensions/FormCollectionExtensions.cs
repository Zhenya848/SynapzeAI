using Microsoft.AspNetCore.Http;

namespace TestsService.Presentation.Extensions;

public static class FormCollectionExtensions
{
    public static string GetString(this IFormCollection form, string key, string defaultValue = "")
    {
        return form.TryGetValue(key, out var value) ? value.ToString().Trim() : defaultValue;
    }

    public static bool GetBool(this IFormCollection form, string key, bool defaultValue = false)
    {
        if (!form.TryGetValue(key, out var value)) 
            return defaultValue;

        var stringValue = value.ToString().Trim().ToLower();
        
        return stringValue switch
        {
            "true" or "1" or "yes" or "on" => true,
            "false" or "0" or "no" or "off" => false,
            _ => bool.TryParse(stringValue, out bool result) ? result : defaultValue
        };
    }

    public static int GetInt(this IFormCollection form, string key, int defaultValue = 0)
    {
        if (!form.TryGetValue(key, out var value)) 
            return defaultValue;

        return int.TryParse(value.ToString(), out int result) ? result : defaultValue;
    }

    public static double GetDouble(this IFormCollection form, string key, double defaultValue = 0)
    {
        if (!form.TryGetValue(key, out var value)) 
            return defaultValue;

        return double.TryParse(value.ToString(), System.Globalization.NumberStyles.Any, 
            System.Globalization.CultureInfo.InvariantCulture, out double result) ? result : defaultValue;
    }

    public static T GetEnum<T>(this IFormCollection form, string key, T defaultValue = default) where T : struct
    {
        if (!form.TryGetValue(key, out var value)) 
            return defaultValue;

        return Enum.TryParse<T>(value.ToString(), true, out T result) ? result : defaultValue;
    }
}