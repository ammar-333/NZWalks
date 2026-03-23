namespace NZWalks.API.Models.Dto
{
    public class ImageUploadDto
    {
        public IFormFile File { get; set; }
        public string FilName { get; set; }
        public string? FileDescription { get; set; }
    }
}
