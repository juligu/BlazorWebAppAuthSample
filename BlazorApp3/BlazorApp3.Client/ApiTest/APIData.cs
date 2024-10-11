
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Text.Json;

namespace BlazorApp3.Client.ApiTest
{
    /// <summary>
    /// Class to consume APIs
    /// </summary>
    /// <param name="httpClientFactory"></param>
    public class APIData(IHttpClientFactory httpClientFactory) : IAPIData
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public async Task<IEnumerable<TestData>> GetTestDataAsync()
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("ApiDataClient");
            var request = new HttpRequestMessage(HttpMethod.Get,
                "api/data");
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await JsonSerializer.DeserializeAsync<List<TestData>>(
           await response.Content.ReadAsStreamAsync()) ?? [];
        }

        public async Task<IEnumerable<TestData>> GetTestRemoteDataAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("ApiDataClient");
            var requestMessage = new HttpRequestMessage(HttpMethod.Get,
                "forward-to-remote/GetData");
            requestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            var response = await httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();

            return await JsonSerializer.DeserializeAsync<List<TestData>>(
               await response.Content.ReadAsStreamAsync()) ?? [];
        }
    }
}
