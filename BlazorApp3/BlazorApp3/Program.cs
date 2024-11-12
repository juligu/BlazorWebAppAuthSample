using Azure.Identity;
using BlazorApp3;
using BlazorApp3.ApiTest;
using BlazorApp3.Client.ApiTest;
using BlazorApp3.Client.Pages;
using BlazorApp3.Components;
using BlazorApp3.Helpers;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

if (builder.Environment.IsDevelopment())
{
    //builder.Configuration.AddAzureKeyVault(
    //new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"),
    //new ClientSecretCredential("TenID", "CID", "CSEC")
    //);
}
else if (builder.Environment.IsProduction())
{
    builder.Configuration.AddAzureKeyVault(
    new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"),
    new DefaultAzureCredential()
    );
}


// Custom Token Storage
builder.Services.AddSingleton<IUserTokenStorage, TokenStorage>();
builder.Services.AddSingleton<CustomTokenStorageEvents, CustomTokenStorageEvents>();

// Add Azure Entra ID auth configuration
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options => {
        builder.Configuration.Bind("AzureAd", options);
        // Get azure app roles from the token
        options.MapInboundClaims = false;
        options.TokenValidationParameters.RoleClaimType = "roles";
        options.TokenValidationParameters.NameClaimType = "name";
        // Explicitly use the Auth Code flow
        options.ResponseType = OpenIdConnectResponseType.Code;
        // Save the token as part of the cookie
        options.SaveTokens = true;
        // Request addional scopes to access APIs
        //options.Scope.Add("api");
        // Custom handler to store tokens, optional
        options.EventsType = typeof(CustomTokenStorageEvents);
    });
// builder.Configuration.GetSection("AzureAd"), 

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();

// Extract information and pass it to the client app
builder.Services.AddScoped<AuthenticationStateProvider, PersistingAuthenticationStateProvider>();
// Service to retrieve data from Server
builder.Services.AddKeyedScoped<IClientData, ServerData>("LocalAPI");


        //builder.Services.AddKeyedScoped<IClientData, RemoteCustomData>("RemoteAPICustom");

// Service to retrieve data from an internal and external APIs
builder.Services.AddScoped<IAPIData, APIDataServer>();
builder.Services.AddScoped<IAPIMData, APIMData>();


        //builder.Services.AddHttpClient("RemoteAPI", httpClient =>
        //{
        //    httpClient.BaseAddress = new Uri("https://localhost:7196");
        //});
        //builder.Services.AddHttpClient("ApiDataClient", httpClient =>
        //{
        //    httpClient.BaseAddress = new Uri("https://localhost:7230");
        //});

// HttpClient to access remote API
builder.Services.AddHttpClient("RemoteAPIClient", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://localhost:7196");
});


builder.Services.AddHttpClient("APIMClient", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://juliguapimdelete.azure-api.net");
    httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", 
        builder.Configuration.GetValue<string>("Ocp-Apim-Subscription-Key")
        );
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorApp3.Client._Imports).Assembly);

// Expose Login and Logout endpoints
app.MapGroup("/authentication").MapLoginAndLogout();

// Endpoint to get data from the server project
app.MapGet("/api/data", ([FromKeyedServices("LocalAPI")] IClientData ServerDataClient) =>
{
    return ServerDataClient.GetData();   
}).RequireAuthorization();

        //app.MapGet("/api/remote", ([FromKeyedServices("RemoteAPI")] IClientData ServerDataClient) =>
        //{
        //    return ServerDataClient.GetData();
        //}).RequireAuthorization();

        //// 
        //app.MapGet("/api/remoteCustom", ([FromKeyedServices("RemoteAPICustom")] IClientData ServerDataClient) =>
        //{
        //    return ServerDataClient.GetData();
        //}).RequireAuthorization();

// Call external API
app.MapGet("/forward-to-remote/GetData", async (IAPIData RemoteDataClient) =>
{
    return Results.Ok(await RemoteDataClient.GetTestRemoteDataAsync());
}).RequireAuthorization();

app.Run();
