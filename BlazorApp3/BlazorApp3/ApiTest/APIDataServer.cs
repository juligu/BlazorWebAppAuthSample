using BlazorApp3.Client.ApiTest;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Http.Logging;
using System.Net.Http;
using System.Text.Json;

namespace BlazorApp3.ApiTest
{
    public class APIDataServer([FromKeyedServices("LocalAPI")] IClientData clientData, 
        IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : IAPIData
    {
        private readonly IClientData _clientData = clientData;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public Task<IEnumerable<TestData>> GetTestDataAsync()
        {
            return _clientData.GetData();
        }

        public async Task<IEnumerable<TestData>> GetTestRemoteDataAsync()
        {
            var httpContext = httpContextAccessor.HttpContext ??
            throw new InvalidOperationException("No HttpContext available from the IHttpContextAccessor!");

            var accessToken = await httpContext.GetTokenAsync("access_token") ??
                throw new InvalidOperationException("No access_token was saved");

            var httpClient = _httpClientFactory.CreateClient("RemoteAPIClient");
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var requestMessage = new HttpRequestMessage(HttpMethod.Get,
                "GetData");
            var response = await httpClient.SendAsync(requestMessage);
            


            response.EnsureSuccessStatusCode();

            return await JsonSerializer.DeserializeAsync<List<TestData>>(
               await response.Content.ReadAsStreamAsync()) ?? [];
        }
    }
}
