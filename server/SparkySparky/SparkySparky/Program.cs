using SparkySparky.Services;
using SparkySparky.DTOs;
using SparkySparky.Models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var mongoConnectionString = builder.Configuration["MongoDb:ConnectionString"]
    ?? throw new InvalidOperationException(
        "MongoDb:ConnectionString is missing.");

builder.Services.AddSingleton<IMongoClient>(
    new MongoClient(mongoConnectionString));
builder.Services.AddSingleton<IMongoHealthChecker, MongoHealthChecker>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "Frontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173", "http://localhost")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddSingleton<IPolygonService, PolygonService>();
builder.Services.AddSingleton<IMapObjectService, MapObjectService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Frontend");
app.MapControllers();

app.Run();

