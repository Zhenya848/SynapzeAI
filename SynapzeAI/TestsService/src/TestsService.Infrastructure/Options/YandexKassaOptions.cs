namespace TestsService.Infrastructure.Options;

public class YandexKassaOptions
{
    public const string YANDEX = "YandexKassa";
    
    public int ShopId { get; init; }
    public string SecretKey { get; init; }
}