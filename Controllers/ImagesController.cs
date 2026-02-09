using Microsoft.AspNetCore.Mvc;
using ToDoList.Services;
using ToDoList.Interfaces;
using ToDoList.Models.DTOs;
using ToDoListModel = ToDoList.Models.Domain.ToDoList;
using AutoMapper;
using ToDoList.CustomActionFilters;
using Microsoft.AspNetCore.Authorization;
using ToDoList.Pagination;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ToDoList.Models.Domain;

namespace ToDoList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImagesController : Controller
    {
        private readonly ILogger<ImagesController> _logger;
        private readonly IImageService _imageService;

        public ImagesController(ILogger<ImagesController> logger, IImageService imageService)
        {
            _logger = logger;
            _imageService = imageService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            ValidateFileUpload(request);

            if (ModelState.IsValid)
            {
                var imageDomainModel = new Image
                {
                    file = request.file,
                    fileExtension = Path.GetExtension(request.file?.FileName),
                    fileSizeInBytes = request.file?.Length ?? 0,
                    fileName = request.fileName,
                    fileDescription = request.fileDescription
                };

                await _imageService.Upload(imageDomainModel);

                return Ok(imageDomainModel);
            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            var allowedExtensions = new string[] {".jpg", ".jpeg", ".png"};

            if (!allowedExtensions.Contains(Path.GetExtension(request.file?.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            if(request.file?.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10Mb");
                
            }
        }

        
    }
}