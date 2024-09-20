using TodoApi.Models;

namespace TodoApi.Repositories.Interfaces;

public interface ITodosRepository
{
    public List<Todo> Get();
    public void Create(Todo todo);
}
