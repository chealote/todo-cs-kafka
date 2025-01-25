using TodoApi.Services.Interfaces;
using TodoApi.Services;
using TodoApi.Repositories.Interfaces;
using TodoApi.Repositories;
using TodoApi.Contexts;

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

builder.Services.AddScoped<ITodosRepository, TodosRepository>();
builder.Services.AddScoped<ITodosService, TodosService>();
builder.Services.AddScoped<IKafkaService, KafkaService>();
builder.Services.AddDbContext<TodoContext>();

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
