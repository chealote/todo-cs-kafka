using TodoApi.Repositories.Interfaces;
using TodoApi.Models;
using System.Reflection;

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

    public Todo? Get(int id)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        return todo;
    }

    public void Create(Todo todo)
    {
        Console.WriteLine($"Create(): id={todo.Id}");
        var existing = _todos.FirstOrDefault(t => t.Id == todo.Id);
        if (existing != null) {
            var maxId = _todos.Max(t => t.Id);
            todo.Id = maxId + 1;
        }
        _todos.Add(todo);
    }

    public bool Delete(int id)
    {
        var existing = _todos.FirstOrDefault(t => t.Id == id);
        if (existing == null)
            return false;
        _todos.Remove(existing);
        return true;
    }

    public void Patch(Todo todo)
    {
        var existing = _todos.FirstOrDefault(t => t.Id == todo.Id);
        foreach (PropertyInfo property in typeof(Todo).GetProperties())
        {
            object? value = property.GetValue(todo);
            property.SetValue(existing, value);
        }
    }
}
