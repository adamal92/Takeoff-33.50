using SparkySparky.Services;
using SparkySparky.DTOs;
using SparkySparky.Models;
using SparkySparky.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

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

app.MapControllers();
app.MapGroup("/api/polygons")
    .WithTags("Polygons")
    .MapMethods("GET", "POST", "DELETE", "/polygons/{id}", async (IMiddleware mediator, HttpContext http, string id) =>
    {
        return Results.NotFound();
    });

app.MapGroup("/api/objects")
    .WithTags("Objects")
    .MapMethods("GET", "POST", "DELETE", "/objects/{id}", async (IMiddleware mediator, HttpContext http, string id) =>
    {
        return Results.NotFound();
    });