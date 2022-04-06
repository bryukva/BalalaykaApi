using Balalayka.Data;
using Balalayka.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IBalalaykaStore, BalalaykaStore>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {Title = "Balalayka API", Version = "v1",});
});
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddControllers();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDbContext<BalalaykaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<BalalaykaDbContext>();
    if (dataContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
    {
        dataContext.Database.Migrate();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/objects/{id}", async (int id, IBalalaykaStore store) =>
{
    var found = await store.Get(id, new CancellationToken());
    return found == null ? Results.NotFound() : Results.Ok(found);
});

app.MapPut("/objects/", async ([FromBody]IReadOnlyList<IDictionary<string, string>> query, [FromServices]IBalalaykaStore store) =>
{
    await store.DeleteAll(new CancellationToken());
    var candidates = query.SelectMany(x => x.Select(a => new BalalaykaCandidate(int.Parse(a.Key), a.Value))).ToList();
    var added = await store.AddList(candidates, new CancellationToken());
    return Results.Ok($"{added} rows added");
});

app.MapGet("/objects/", async ([FromQuery]int? codeLowerLimit, [FromQuery]int? codeUpperLimit, [FromQuery]string? valueMask, IBalalaykaStore store) =>
{
    var dataset = await store.GetList(new BalalaykasFilter(codeUpperLimit, codeLowerLimit, valueMask), new CancellationToken());
    return Results.Ok(dataset);
});

app.Run();

public partial class Program { } //for unit tests with inMemoryDb