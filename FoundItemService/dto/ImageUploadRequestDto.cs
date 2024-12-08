using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoundItemService.dto
{
    public class ImageUploadRequestDto
    {
        [Required]
        public IFormFile File { get; set; }
  
    }
}
