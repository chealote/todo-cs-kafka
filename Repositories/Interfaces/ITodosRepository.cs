using TodoApi.Models;

namespace TodoApi.Repositories.Interfaces;

public interface ITodosRepository
{
    public List<Todo> Get();
    public Todo? Get(int id);
    public void Create(Todo todo);
    public bool Delete(int id);
    public void Patch(Todo todo);
}
