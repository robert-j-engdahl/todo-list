using System.Linq;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using TodoList.AddNewTodoItem;
using TodoList.Common.Database;
using TodoList.ShowTodoList;
using Xunit;

namespace TodoList.Tests.ShowTodoList
{
    public class ShowTodoListViewModelTest
    {
        private readonly ShowTodoListViewModel _target;

        private readonly DbContextOptions<TodoDbContext> _dbContextOptions = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase("show todo items view model test").Options;

        private readonly INewTodoItemViewModel _newTodoItemViewModel = Substitute.For<INewTodoItemViewModel>();

        public ShowTodoListViewModelTest()
        {
            _target = new ShowTodoListViewModel(CreateDbContext);

            _target.Observe(_newTodoItemViewModel);
        }

        private TodoDbContext CreateDbContext()
        {
            return new TodoDbContext(_dbContextOptions);
        }

        [Fact]
        public void Loads_TodoList_When_Initialized()
        {
            var todoItem = new TodoItem { Text = "A TODO" };
            using (var dbContext = CreateDbContext())
            {
                dbContext.TodoItems.Add(todoItem);
                dbContext.SaveChanges();
            }

            _target.PropertyChanged += (sender, args) => { Assert.Contains(todoItem.Id, _target.TodoList.Select(t => t.Id)); };

            Assert.PropertyChanged(_target, nameof(_target.TodoList), () =>
                _target.Initialize());
        }

        [Fact]
        public void Reloads_TodoList_When_New_TodoItem_Added()
        {
            var todoItem = new TodoItem { Text = "A TODO" };
            using (var dbContext = CreateDbContext())
            {
                dbContext.TodoItems.Add(todoItem);
                dbContext.SaveChanges();
            }

            _target.PropertyChanged += (sender, args) => { Assert.Contains(todoItem.Id, _target.TodoList.Select(t => t.Id)); };

            Assert.PropertyChanged(_target, nameof(_target.TodoList), () =>
                _newTodoItemViewModel.TodoItemAdded += Raise.EventWith<TodoItemAddedEventArgs>());
        }
    }
}