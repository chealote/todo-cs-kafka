using Microsoft.EntityFrameworkCore;
using Confluent.Kafka;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "TodoAPI";
    config.Title = "TodoAPI v1";
    config.Version = "v1";
});
var app = builder.Build();

var kafkaClient = new Kafka("kafka-topic", new ConsumerConfig {
    GroupId = "kafka-consumer-group",
    BootstrapServers = "localhost:9092",
    AutoOffsetReset = AutoOffsetReset.Earliest,
});

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "TodoAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.MapGet("/todoitems", async (TodoDb db) =>
    await db.Todos.ToListAsync());

app.MapGet("/todoitems/complete", async (TodoDb db) =>
    await db.Todos.Where(t => t.IsComplete).ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, TodoDb db) =>
    await db.Todos.FindAsync(id)
        is Todo todo
            ? Results.Ok(todo)
            : Results.NotFound());

app.MapGet("/kafka/consume", (TodoDb db) =>
{
    kafkaClient.StartConsuming(async (string todoString) => {
        var todoRequest = JsonConvert.DeserializeObject<TodoRequest>(todoString);
        if (todoRequest == null) {
            return;
        }
        var todo = Mapper.Map<Todo, TodoRequest>(todoRequest, Mappers.TodoMapper);
        Todo? existing = null;
        if (todoRequest.Id != null) {
            existing = await db.Todos.FindAsync(todoRequest.Id);

            if (existing is null) {
                Console.WriteLine($"resource {todo.Id} not found");
                return;
            }
        }
        switch(todoRequest.Action) {
            case EAction.Create:
                db.Todos.Add(todo);
                break;
            case EAction.Update:
                existing.Name = todo.Name;
                existing.IsComplete = todo.IsComplete;
                break;
            case EAction.Patch:
                existing.IsComplete = true;
                break;
            case EAction.Delete:
                db.Todos.Remove(existing);
                await db.SaveChangesAsync();
                break;
        }
        await db.SaveChangesAsync();
    });
});

app.MapPost("/kafka/produce", async (TodoRequest todo) =>
{
    await kafkaClient.Produce(JsonConvert.SerializeObject(todo));
    return Results.NoContent();
});

app.Run();
