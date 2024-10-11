
using System.Net.Http.Json;

namespace BlazorApp3.Client.ApiTest;
public sealed class ClientData(HttpClient httpClient) : IClientData
{
    public async Task<IEnumerable<TestData>> GetData()
    {
        throw new NotImplementedException();
        //return await httpClient.GetFromJsonAsync<TestData[]>("/api/data") ??
        // throw new IOException("No client");
    }
}
