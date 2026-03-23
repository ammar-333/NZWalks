using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.Dto;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImageController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }


        // POST: api/Image/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadDto imageUploadDto)
        {
            ValidateImage(imageUploadDto.File);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ImageDomain = new Image
            {
                File = imageUploadDto.File,
                FileExtenstion = Path.GetExtension(imageUploadDto.File.FileName),
                FileSizeInByte = imageUploadDto.File.Length,
                FileName = imageUploadDto.FilName,
                FileDescription = imageUploadDto.FileDescription
            };

            ImageDomain = await imageRepository.Upload(ImageDomain);

            return Ok(ImageDomain);
        }

        private void ValidateImage(IFormFile File)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(File.FileName).ToLower()))
            {
                ModelState.AddModelError("File", "unsapported file extension");
            }

            if (File.Length > 10 * 1024 * 1024)
            {
                ModelState.AddModelError("File", "file size is bigger than 10MG");
            }
        }
    }
}
