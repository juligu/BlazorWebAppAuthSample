using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add cors policies for securing the API
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin().
                AllowAnyHeader().
                AllowAnyMethod();
        });
});

// Add auth for external API
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://login.microsoftonline.com/c82c56b9-839e-46a0-927f-a8a30231905a/";
        options.Audience = "api://904f9492-cad5-4b9a-b7e8-df6a95de7f65";
        options.MapInboundClaims = false;
        options.TokenValidationParameters.RoleClaimType = "roles";
        options.TokenValidationParameters.NameClaimType = "name";
    });

builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();


var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast").RequireAuthorization()
.WithOpenApi();

// Endpoint for get data, requires a valid session
app.MapGet("/GetData", () =>
{
    var data = Enumerable.Range(1, 5).Select(index =>
                new TestData(id: index, name: $"Item{index}")
            ).ToArray();
    return data;
})
.WithName("GetData").RequireAuthorization()
.WithOpenApi();

// Endpoint for get data, requires a valid session and the user has to be an Admin
app.MapGet("/GetDataSecond", () =>
{
    var data = Enumerable.Range(1, 5).Select(index =>
                new TestData(id: index, name: $"Item{index}")
            ).ToArray();
    return data;
})
.WithName("GetDataSecond").RequireAuthorization(p => p.RequireRole("Admin"))
.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

internal class TestData(int id, string name)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
}
