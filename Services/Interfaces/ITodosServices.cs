using TodoApi.Models;

namespace TodoApi.Services.Interfaces;

public interface ITodosService
{
    public List<Todo> GetTodos();
    public void CreateTodo(Todo todo);
}
