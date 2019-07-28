using System.ComponentModel;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using TodoList.AddNewTodoItem;
using TodoList.Common.Database;
using TodoList.ShowTodoList;

namespace TodoList
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                using (var dbContext = new TodoDbContext())
                {
                    dbContext.Database.Migrate();
                }

                var newTodoItemViewModel = new NewTodoItemViewModel(() => new TodoDbContext());
                NewTodoItemView.DataContext = newTodoItemViewModel;
                var showTodoListViewModel = new ShowTodoListViewModel(() => new TodoDbContext());
                ShowTodoListView.DataContext = showTodoListViewModel;

                showTodoListViewModel.Observe(newTodoItemViewModel);
                showTodoListViewModel.Initialize();
            }
        }
    }
}
