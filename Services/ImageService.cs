using ToDoList.Database;
using ToDoList.Interfaces;
using ToDoList.Models.Domain;

namespace ToDoList.Services;

public class ImageService : IImageService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ToDoListContext _toDoListContext;
    public ImageService(
        IWebHostEnvironment webHostEnvironment, 
        IHttpContextAccessor httpContextAccessor,
        ToDoListContext toDoListContext
        )
    {
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
        _toDoListContext = toDoListContext;
    }
    public async Task<Image> Upload(Image image)
    {
        var localFilePath = Path.Combine(
        _webHostEnvironment.ContentRootPath
        , "Images"
        , $"{image.fileName}{image.fileExtension}"
        );

        using var stream = new FileStream(localFilePath, FileMode.Create);

        if (image.file == null)
            throw new ArgumentNullException(nameof(image.file), "File cannot be null");

        await image.file.CopyToAsync(stream);

        var scheme          = _httpContextAccessor.HttpContext?.Request.Scheme;
        var host            = _httpContextAccessor.HttpContext?.Request.Host;
        var pathBase        = _httpContextAccessor.HttpContext?.Request.PathBase;

        var fileName        = image.fileName;
        var fileExtension   = image.fileExtension;


        var urlFilePath = $"{scheme}://{host}{pathBase}/Images/{fileName}{fileExtension}";

        image.filePath = urlFilePath;

        _toDoListContext.Images.Add(image);
        await _toDoListContext.SaveChangesAsync();           

        return image;
    }
}