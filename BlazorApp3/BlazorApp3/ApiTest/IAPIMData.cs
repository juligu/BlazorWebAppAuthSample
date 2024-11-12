
namespace BlazorApp3.ApiTest
{
    public interface IAPIMData
    {
        IHttpClientFactory HttpClientFactory { get; }

        Task<WeatherForecast[]> GetData();
    }
}