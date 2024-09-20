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

    [HttpGet("subscribe")]
    public void Subscribe()
    {
        _kafkaService.Subscribe((string jsonTodo) => {
            var todo = JsonSerializer.Deserialize<Todo>(jsonTodo);
            if (todo == null) {
                Console.WriteLine($"Got null todo from kafka");
                return;
            }
            _todosService.CreateTodo(todo);
        });
    }

    [HttpPost("")]
    public IActionResult CreateTodo(Todo todo)
    {
        try
        {
            _kafkaService.Produce(JsonSerializer.Serialize(todo));
            return Accepted();
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error CreateTodo(): {e}");
            return StatusCode(500);
        }
    }
}
