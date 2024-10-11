using BlazorApp3.Client.ApiTest;
using BlazorApp3.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using System.Linq;
using System.Net.Http;

namespace BlazorApp3.ApiTest
{
    public class RemoteData(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : IClientData
    {
        public async Task<IEnumerable<TestData>> GetData()
        {
            HttpClient httpClient = httpClientFactory.CreateClient("RemoteAPI");
            var httpContext = httpContextAccessor.HttpContext ??
            throw new InvalidOperationException("No HttpContext available from the IHttpContextAccessor!");

            var accessToken = await httpContext.GetTokenAsync("access_token") ??
                throw new InvalidOperationException("No access_token was saved");

            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/GetData");
            requestMessage.Headers.Authorization = new("Bearer", accessToken);
            using var response = await httpClient.SendAsync(requestMessage);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TestData[]>() ??
                throw new IOException("No remote service!");
        }
    }
}
