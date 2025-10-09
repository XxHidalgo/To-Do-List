
namespace ToDoList.Models;

public class ToDoList
{
    public int id { get; set; }
    public int user_id { get; set; }
    public string? title { get; set; }
    public string? description { get; set; }
    public bool isCompleted { get; set; }
    public virtual User? user { get; set; }
    public virtual ICollection<ToDoTask>? tasks { get; set; }
}