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

app.UseHttpsRedirection();
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
app.MapCartEndpoints(apiVersionSet);
app.MapOrderEndpoints(apiVersionSet);


using (var scope = app.Services.CreateAsyncScope())
{
    var databaseSeeder = scope.ServiceProvider
        .GetRequiredService<DatabaseSeeder>();

    await databaseSeeder.SeedAllAsync();
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}





app.Run();

