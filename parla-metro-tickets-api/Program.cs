using DotNetEnv;
using parla_metro_tickets_api.src.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Env.Load();

var mongoSettings = new MongoDbSettings
{
    ConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING")!,
    DatabaseName = Environment.GetEnvironmentVariable("MONGO_DATABASE")!
};

builder.Services.AddSingleton(new MongoDbContext(mongoSettings));

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();