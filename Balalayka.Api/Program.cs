using Balalayka.Data;
using Balalayka.Domain.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IBalalaykaStore, BalalaykaStore>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {Title = "Balalayka API", Version = "v1",});
});

builder.Services.AddControllers();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDbContext<BalalaykaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<BalalaykaDbContext>();
    dataContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/objects/{id}", async (int id, IBalalaykaStore store) => await store.Get(id, new CancellationToken()));
//TODO other methods
app.MapPut("/objects/", async (AddObjectListQuery query, IBalalaykaStore store) => await store.Get(0, new CancellationToken()));

app.Run();