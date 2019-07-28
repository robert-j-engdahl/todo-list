using Microsoft.EntityFrameworkCore;

namespace TodoList.Common.Database
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
        }

        public TodoDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=todo-list.db");
            }
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}