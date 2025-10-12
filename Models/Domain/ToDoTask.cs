
namespace ToDoList.Models.Domain;

public class ToDoTask
{
    public int id { get; set; }
    public int toDoList_id { get; set; }
    public string? title { get; set; }
    public string? description { get; set; }
    public DateTime dueDate { get; set; }
    public bool isCompleted { get; set; }
    public virtual ToDoList? toDoList { get; set; }
}