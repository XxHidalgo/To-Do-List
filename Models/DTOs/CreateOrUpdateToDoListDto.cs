using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models.DTOs
{
    public class CreateOrUpdateToDoListDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int user_id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? title { get; set; }

        [MaxLength(300)]
        public string? description { get; set; }

        public bool isCompleted { get; set; }
    }
}
