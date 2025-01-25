using TodoApi.Repositories.Interfaces;
using TodoApi.Models;
using TodoApi.Contexts;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Repositories;

public class TodosRepository : ITodosRepository
{
    private readonly TodoContext _context;

    public TodosRepository(TodoContext context)
    {
        _context = context;
    }

    public List<Todo> Get()
    {
        return _context.Todos.ToList();
    }

    public Todo? Get(int id)
    {
        return _context.Todos.Find(id);
    }

    public void Create(Todo todo)
    {
        _context.Todos.Add(todo);
        _context.SaveChanges();
    }

    public bool Delete(int id)
    {
        var todo = _context.Todos.Find(id);
        if (todo == null)
            return false;

        _context.Todos.Remove(todo);
        _context.SaveChanges();
        return true;
    }

    public void Patch(Todo todo)
    {
        var existing = _context.Todos.Find(todo.Id);
        if (existing == null)
            return;

        _context.Entry(existing).CurrentValues.SetValues(todo);
        _context.SaveChanges();
    }
}
