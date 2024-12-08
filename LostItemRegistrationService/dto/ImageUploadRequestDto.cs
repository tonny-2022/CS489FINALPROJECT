using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LostItemRegistrationService.dto
{
    public class ImageUploadRequestDto
    {
        [Required]
        public IFormFile File { get; set; }
  
    }
}
