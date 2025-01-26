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
            if (_config.GetValue<bool>("UseInMemoryDatabase"))
            {
                options.UseInMemoryDatabase("DB");
            }
            else
            {
                options.UseSqlite(_config.GetConnectionString("SqliteFilepath"));
            }
        }

        public DbSet<Todo> Todos { get; set; }
    }
}
