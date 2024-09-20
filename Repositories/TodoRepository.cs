using TodoApi.Repositories.Interfaces;
using TodoApi.Models;

namespace TodoApi.Repositories;

public class TodosRepository : ITodosRepository
{
    private List<Todo> _todos;

    public TodosRepository()
    {
        Console.WriteLine("Constructor()");
        _todos = new List<Todo>();
    }

    public List<Todo> Get()
    {
        return _todos;
    }

    public void Create(Todo todo)
    {
        _todos.Add(todo);
    }
}
