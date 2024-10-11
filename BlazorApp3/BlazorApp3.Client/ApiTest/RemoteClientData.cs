
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Text.Json;

namespace BlazorApp3.Client.ApiTest;

public class RemoteClientData(HttpClient httpClient) : IClientData
{
    public async Task<IEnumerable<TestData>> GetData()
    {
        throw new NotImplementedException();
        //httpClient.BaseAddress = new Uri("https://localhost:7230");
        //var request = new HttpRequestMessage(HttpMethod.Get,
        //    "/forward-to-remote/GetData");
        //request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        //var response = await httpClient.SendAsync(request);
        //response.EnsureSuccessStatusCode();

        //return await JsonSerializer.DeserializeAsync<List<TestData>>(
        //    await response.Content.ReadAsStreamAsync()) ?? [];
    }
}
