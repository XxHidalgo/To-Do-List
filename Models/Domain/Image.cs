
using System.ComponentModel.DataAnnotations.Schema;
using ToDoList.Enums;

namespace ToDoList.Models.Domain;

public class Image
{
    public int id { get; set; }
    [NotMapped]
    public IFormFile? file { get; set; }
    public int idReference { get; set; }
    public string? fileName { get; set; }
    public string? fileDescription { get; set; }
    public string? fileExtension { get; set; }
    public long fileSizeInBytes { get; set; }
    public string? filePath { get; set; }
    public CategoryImageEnum fileCategory { get; set; }
}