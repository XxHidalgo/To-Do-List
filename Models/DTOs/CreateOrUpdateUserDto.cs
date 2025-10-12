namespace ToDoList.Models.DTOs
{
    public class CreateOrUpdateUserDto
    {
        public string? username { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
    }
}
