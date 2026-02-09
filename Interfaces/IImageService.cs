using ToDoList.Models.Domain;

namespace ToDoList.Interfaces;
public interface IImageService
{
   Task<Image> Upload(Image image);
}