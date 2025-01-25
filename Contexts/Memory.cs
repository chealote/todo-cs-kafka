using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Contexts
{
  public class TodoContext : DbContext
  {
    protected readonly IConfiguration _config;

    public TodoContext(IConfiguration config)
    {
      _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      options.UseInMemoryDatabase("DB");
    }

    public DbSet<Todo> Todos { get; set; }
  }
}
