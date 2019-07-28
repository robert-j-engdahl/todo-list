using System;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using TodoList.AddNewTodoItem;
using TodoList.Common.Database;
using Xunit;

namespace TodoList.Tests.AddNewTodoItem
{
    public class NewTodoItemViewModelTest
    {
        private readonly DbContextOptions<TodoDbContext> _dbContextOptions = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase("new todo item view model test").Options;

        private readonly NewTodoItemViewModel _target;

        public NewTodoItemViewModelTest()
        {
            _target = new NewTodoItemViewModel(CreateDbContext);
        }

        private TodoDbContext CreateDbContext()
        {
            return new TodoDbContext(_dbContextOptions);
        }

        [Fact]
        public void Add_Saves_Current_TodoItem()
        {
            var oldTodoItem = _target.TodoItem;
            _target.TodoItem.Text = "TODO TEXT";
            _target.Add();

            Assert.NotEqual(oldTodoItem, _target.TodoItem);

            using (var dbContext = CreateDbContext())
            {
                var addedTodoItem = dbContext.TodoItems.Find(oldTodoItem.Id);
                Assert.Equal("TODO TEXT", addedTodoItem.Text);
            }
        }

        [Fact]
        public void Add_Sets_New_TodoItem()
        {
            var oldTodoItem = _target.TodoItem;
            Assert.PropertyChanged(_target, nameof(_target.TodoItem), () =>
            {
                _target.PropertyChanged += (sender, args) => Assert.NotEqual(oldTodoItem, _target.TodoItem);
                _target.TodoItem.Text = "TODO TEXT";
                _target.Add();
            });
        }

        [Fact]
        public void Add_Notifies_About_New_TodoItem_Added()
        {
            var oldTodoItem = _target.TodoItem;
            _target.TodoItem.Text = "TODO TEXT";
            var listener = Substitute.For<EventHandler<TodoItemAddedEventArgs>>();
            _target.TodoItemAdded += listener;
            listener.WhenForAnyArgs(l => l.Invoke(null, null)).Do(callInfo =>
            {
                using (var dbContext = CreateDbContext())
                {
                    var addedTodoItem = dbContext.TodoItems.Find(oldTodoItem.Id);
                    Assert.Equal("TODO TEXT", addedTodoItem.Text);
                }
            });

            _target.Add();

            listener.Received().Invoke(Arg.Is(_target), Arg.Any<TodoItemAddedEventArgs>());
        }
    }
}