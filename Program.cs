using TodoApi.Services.Interfaces;
using TodoApi.Services;
using TodoApi.Repositories.Interfaces;
using TodoApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient();

builder.Services.AddControllers();

builder.Services.AddTransient<ITodosService, TodosService>();
builder.Services.AddSingleton<ITodosRepository, TodosRepository>();
builder.Services.AddSingleton<IKafkaService, KafkaService>();

var app = builder.Build();
app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.DocumentTitle = "HelloWorld";
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
});

app.MapControllers();

app.Run();
