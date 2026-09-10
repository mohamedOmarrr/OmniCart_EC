using Asp.Versioning;
using E_commerce_infrastructure;
using E_commerce_infrastructure.Seeding;
using E_Commerce_persentation;
using E_Commerce_persentation.EndPoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddPresentation();
builder.Services.AddInfrastructure(builder.Configuration);



var app = builder.Build();


app.UseAuthentication();

app.UseAuthorization();

var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1, 0))
    .ReportApiVersions()
    .Build();


app.MapProductEndpoints(apiVersionSet);
app.MapUserEndpoints(apiVersionSet);
app.MapAuthEndpoints(apiVersionSet);
app.MapBrandEndpoints(apiVersionSet);
app.MapCategoryEndpoints(apiVersionSet);
app.MapWishlistEndpoints(apiVersionSet);

using (var scope = app.Services.CreateAsyncScope())
{
    var databaseSeeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    await databaseSeeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

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
    .WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}