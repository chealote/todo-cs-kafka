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

    public bool UpdateTodo(Todo todo)
    {
        Console.WriteLine("UpdateTodos()");
        var ok = _todosRepository.Delete(todo.Id);
        if (!ok)
            return false;
        _todosRepository.Create(todo);
        return true;
    }

    public bool DeleteTodo(int id)
    {
        Console.WriteLine("DeleteTodos()");
        return _todosRepository.Delete(id);
    }

    public bool CompleteTodo(int id)
    {
        Console.WriteLine("PatchTodos()");
        var todo = _todosRepository.Get(id);
        if (todo == null)
            return false;
        todo.IsComplete = true;
        _todosRepository.Patch(todo);
        return true;
    }
}
