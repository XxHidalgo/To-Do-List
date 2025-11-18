using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models.DTOs
{
    public class CreateOrUpdateAuthDto
    {
        [Required]
        [MaxLength(30)]
        public string? username { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string? password { get; set; }
        public string[]? Roles { get; set; }
    }
}
