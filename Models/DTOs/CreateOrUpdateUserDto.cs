using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models.DTOs
{
    public class CreateOrUpdateUserDto
    {
        [Required]
        [MaxLength(30)]
        public string? username { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? email { get; set; }

        [StringLength(30, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string? password { get; set; }
    }
}
