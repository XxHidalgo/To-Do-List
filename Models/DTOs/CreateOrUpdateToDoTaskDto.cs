using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models.DTOs
{
    public class CreateOrUpdateToDoTaskDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int toDoList_id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? title { get; set; }

        [MaxLength(300)]
        public string? description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime dueDate { get; set; }

        public bool isCompleted { get; set; }
    }
}
