using System;
using System.ComponentModel;
using TodoList.Common.Database;

namespace TodoList.AddNewTodoItem
{
    public interface INewTodoItemViewModel
    {
        TodoItem TodoItem { get; set; }
        void Add();
        event EventHandler<TodoItemAddedEventArgs> TodoItemAdded;
    }

    public class NewTodoItemViewModel : INotifyPropertyChanged, INewTodoItemViewModel
    {
        private readonly Func<TodoDbContext> _createDbContext;

        public NewTodoItemViewModel(Func<TodoDbContext> createDbContext)
        {
            _createDbContext = createDbContext;
        }

        public TodoItem TodoItem { get; set; } = new TodoItem();

        public void Add()
        {
            using (var dbContext = _createDbContext())
            {
                dbContext.TodoItems.Add(TodoItem);
                dbContext.SaveChanges();
            }
            TodoItem = new TodoItem();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TodoItem)));
            TodoItemAdded?.Invoke(this, new TodoItemAddedEventArgs());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<TodoItemAddedEventArgs> TodoItemAdded;
    }
}