using System.ComponentModel.DataAnnotations;

namespace TodoList.Common.Database
{
    public class TodoItem
    {
        [Key]
        public int Id { get; set; }

        public string Text { get; set; }
    }
}