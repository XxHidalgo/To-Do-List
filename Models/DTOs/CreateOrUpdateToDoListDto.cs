namespace ToDoList.Models.DTOs
{
    public class CreateOrUpdateToDoListDto
    {
        public int user_id { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public bool isCompleted { get; set; }
    }
}
