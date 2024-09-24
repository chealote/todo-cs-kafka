using TodoApi.Models;

namespace TodoApi.Services.Interfaces;

public interface ITodosService
{
    public List<Todo> GetTodos();
    public void CreateTodo(Todo todo);
    public bool UpdateTodo(Todo todo);
    public bool DeleteTodo(int id);
    public bool CompleteTodo(int id);
}
