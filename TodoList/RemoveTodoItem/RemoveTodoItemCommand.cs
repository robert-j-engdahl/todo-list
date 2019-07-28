using System;
using System.Windows.Input;
using TodoList.Common.Database;

namespace TodoList.RemoveTodoItem
{
    public class RemoveTodoItemCommand : ICommand
    {
        private readonly Func<TodoDbContext> _createDbContext;

        public RemoveTodoItemCommand(Func<TodoDbContext> createDbContext)
        {
            _createDbContext = createDbContext;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            using (var dbContext = _createDbContext())
            {
                dbContext.TodoItems.Remove((TodoItem)parameter);
                dbContext.SaveChanges();
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}