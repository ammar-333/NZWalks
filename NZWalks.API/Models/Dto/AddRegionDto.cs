using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.Dto
{
    public class AddRegionDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Minimum length is 3 char")]
        [MaxLength(3, ErrorMessage = "Maximum length is 3 char")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Maximum length is 100 characters")]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
