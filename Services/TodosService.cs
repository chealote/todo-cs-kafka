using TodoApi.Services.Interfaces;
using TodoApi.Repositories.Interfaces;
using TodoApi.Repositories;
using TodoApi.Models;

namespace TodoApi.Services;

public class TodosService : ITodosService
{
    private ITodosRepository _todosRepository;

    public TodosService(ITodosRepository todosRepository)
    {
        Console.WriteLine("Constructor()");
        _todosRepository = todosRepository;
    }

    public List<Todo> GetTodos()
    {
        Console.WriteLine("GetTodos()");
        return _todosRepository.Get();
    }

    public void CreateTodo(Todo todo)
    {
        Console.WriteLine("CreateTodos()");
        _todosRepository.Create(todo);
    }
}
