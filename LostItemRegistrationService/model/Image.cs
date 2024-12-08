using System.ComponentModel.DataAnnotations.Schema;

namespace LostItemRegistrationService.model
{
	public class Image
	{
		public IFormFile File { get; set; }
		
		public string FileName { get; set; }
		public string FilePath { get; set; }
		public string FileExtension { get; set; }
	}
}
