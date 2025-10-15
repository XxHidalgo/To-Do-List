namespace ToDoList.Models.DTOs
{
    public class ToDoListDto
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public bool isCompleted { get; set; }
        public virtual UserDto? user { get; set; }
        public virtual ICollection<ToDoTaskDto>? tasks { get; set; }
    }
}
