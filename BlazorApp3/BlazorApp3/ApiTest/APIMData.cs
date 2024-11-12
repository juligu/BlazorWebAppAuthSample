using BlazorApp3.Client.ApiTest;
using System.Net.Http;

namespace BlazorApp3.ApiTest;

public class APIMData(IHttpClientFactory httpClientFactory) : IAPIMData
{
    public IHttpClientFactory HttpClientFactory { get; } = httpClientFactory;

    public async Task<WeatherForecast[]> GetData()
    {
        var httpClient = HttpClientFactory.CreateClient("APIMClient");
        
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/api/weatherforecast");
        using var response = await httpClient.SendAsync(requestMessage);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<WeatherForecast[]>() ??
            throw new IOException("No remote service!");
    }
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}