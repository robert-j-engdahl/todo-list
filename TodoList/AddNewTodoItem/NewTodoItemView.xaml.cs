using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TodoList.AddNewTodoItem
{
    public partial class NewTodoItemView : UserControl
    {
        public NewTodoItemView()
        {
            InitializeComponent();
        }

        private void Add_TodoItem(object sender, RoutedEventArgs e)
        {
            ViewModel.Add();
        }

        public NewTodoItemViewModel ViewModel => DataContext as NewTodoItemViewModel;

        private void Add_TodoItem_On_Enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control))
            {
                ViewModel.Add();

                e.Handled = true;
            }
        }

        private void NewTodoItemView_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control))
            {
                e.Handled = true;
            }
        }

        private void Add_NewLine_On_CtrlEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control))
            {
                // https://stackoverflow.com/a/10549299/2154774
                var caretIndex = TextBox.CaretIndex;
                TextBox.Text = TextBox.Text.Insert(caretIndex, System.Environment.NewLine);
                TextBox.CaretIndex = caretIndex + 1;

                e.Handled = true;
            }
        }
    }
}