namespace ToDoList.Models.DTOs
{
    public class ToDoTaskDto
    {
        public int id { get; set; }
        public int toDoList_id { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public DateTime dueDate { get; set; }
        public bool isCompleted { get; set; }
    }
}
