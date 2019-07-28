using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using TodoList.Common.Database;
using TodoList.RemoveTodoItem;
using Xunit;

namespace TodoList.Tests.RemoveTodoItem
{
    public class RemoveTodoItemCommandTest
    {
        private readonly RemoveTodoItemCommand _target;

        private readonly DbContextOptions<TodoDbContext> _dbContextOptions = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase("remove todo item command test").Options;

        public RemoveTodoItemCommandTest()
        {
            _target = new RemoveTodoItemCommand(CreateDbContext);
        }

        private TodoDbContext CreateDbContext()
        {
            return new TodoDbContext(_dbContextOptions);
        }

        [Fact]
        public void Is_An_ICommand()
        {
            Assert.IsAssignableFrom<ICommand>(_target);
        }

        [Fact]
        public void Deletes_Given_TodoItem()
        {
            var todoItem = new TodoItem();
            using (var dbContext = CreateDbContext())
            {
                dbContext.TodoItems.Add(todoItem);
                dbContext.SaveChanges();
            }

            _target.Execute(todoItem);

            using (var dbContext = CreateDbContext())
            {
                Assert.Null(dbContext.TodoItems.Find(todoItem.Id));
            }
        }
    }
}