using System.Net;
using DotNetEnv;
using Microsoft.AspNetCore.Mvc.Routing;
using parla_metro_tickets_api.src.Data;
using parla_metro_tickets_api.src.Interfaces;
using parla_metro_tickets_api.src.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Env.Load();

var mongoSettings = new MongoDbSettings
{
    ConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING") ?? throw new InvalidOperationException("MONGO_CONNECTION_STRING environment variable is not set."),
    DatabaseName = Environment.GetEnvironmentVariable("MONGO_DATABASE") ?? throw new InvalidOperationException("MONGO_DATABASE environment variable is not set.")
};

builder.Services.AddSingleton(new MongoDbContext(mongoSettings));

builder.Services.AddControllers();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.MapControllers();
app.Run();