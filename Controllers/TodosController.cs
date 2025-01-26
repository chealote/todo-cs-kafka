using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services.Interfaces;
using System.Text.Json;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodosController : ControllerBase
{
    private ITodosService _todosService;
    private IKafkaService _kafkaService;

    public TodosController(ITodosService todosService, IKafkaService kafkaService)
    {
        _todosService = todosService;
        _kafkaService = kafkaService;
    }

    [HttpGet("")]
    public List<Todo> GetAllTodos()
    {
        return _todosService.GetTodos();
    }

    // TODO subscribe should stop at some point, after doing some
    // operations?
    [HttpGet("subscribe")]
    public void Subscribe()
    {
        _kafkaService.Subscribe((string jsonTodo) =>
        {
            var todoRequest = JsonSerializer.Deserialize<TodoRequest>(jsonTodo);
            if (todoRequest == null)
            {
                Console.WriteLine($"Got null todo from kafka");
                return;
            }
            var todo = Mappers.TodoMapper(todoRequest);
            Console.WriteLine($"Got action from todo {todoRequest.Action}");
            switch (todoRequest.Action)
            {
                case ActionEnum.Create:
                    _todosService.CreateTodo(todo);
                    break;
                case ActionEnum.Update:
                    _todosService.UpdateTodo(todo);
                    break;
                case ActionEnum.Delete:
                    _todosService.DeleteTodo(todo.Id);
                    break;
                case ActionEnum.Patch:
                    _todosService.CompleteTodo(todo.Id);
                    break;
            }
        });
    }

    // TODO shouldn't allow Id to be set when creating a Todo, that's
    // for the Db to do
    [HttpPost("")]
    public IActionResult CreateTodo(TodoRequest todo)
    {
        try
        {
            _kafkaService.Produce(JsonSerializer.Serialize(todo));
            return Accepted();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error CreateTodo(): {e}");
            return StatusCode(500);
        }
    }
}
