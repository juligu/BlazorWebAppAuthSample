using BlazorApp3.Client.ApiTest;
using BlazorApp3.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;

namespace BlazorApp3.ApiTest;

public class RemoteCustomData(AuthenticationStateProvider authenticationState, IUserTokenStorage userTokenStorage, IHttpClientFactory httpClientFactory)
    : IClientData
{
    private readonly AuthenticationStateProvider _authenticationState = authenticationState;

    public async Task<IEnumerable<TestData>> GetData()
    {
        var authState = await _authenticationState.GetAuthenticationStateAsync();
        var claimsPrincipal = authState.User;
        var accessToken = await userTokenStorage.GetTokenAsync(claimsPrincipal);
        HttpClient httpClient = httpClientFactory.CreateClient("RemoteAPI");
        //var httpContext = httpContextAccessor.HttpContext ??
        //throw new InvalidOperationException("No HttpContext available from the IHttpContextAccessor!");

        //var accessToken = await httpContext.GetTokenAsync("access_token") ??
        //    throw new InvalidOperationException("No access_token was saved");

        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/GetData");
        requestMessage.Headers.Authorization = new("Bearer", accessToken.AccessToken);
        using var response = await httpClient.SendAsync(requestMessage);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TestData[]>() ??
            throw new IOException("No remote service!");
    }
}
