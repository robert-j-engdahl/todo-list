using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TodoList.AddNewTodoItem;
using TodoList.Common.Database;

namespace TodoList.ShowTodoList
{
    public class ShowTodoListViewModel : INotifyPropertyChanged
    {
        private readonly Func<TodoDbContext> _createDbContext;
        private ICollection<TodoItem> _todoList;

        public ShowTodoListViewModel(Func<TodoDbContext> createDbContext)
        {
            _createDbContext = createDbContext;
        }

        public void Observe(INewTodoItemViewModel newTodoItemViewModel)
        {
            newTodoItemViewModel.TodoItemAdded += (sender, args) => ReloadTodoList();
        }

        public void Initialize()
        {
            ReloadTodoList();
        }

        private void ReloadTodoList()
        {
            using (var dbContext = _createDbContext())
            {
                TodoList = dbContext.TodoItems.ToList();
            }
        }

        public ICollection<TodoItem> TodoList
        {
            get => _todoList;
            set
            {
                _todoList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TodoList)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}