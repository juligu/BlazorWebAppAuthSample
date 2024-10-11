using BlazorApp3.Client;
using BlazorApp3.Client.ApiTest;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add Authentication 
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
// Add class to read user data persisted from the server
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

// Service to access API
builder.Services.AddScoped<IAPIData, APIData>();

    //builder.Services.AddHttpClient<ClientData>(httpClient =>
    //{
    //    httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    //});

    //builder.Services.AddScoped<IClientData, ClientData>();


await builder.Build().RunAsync();
