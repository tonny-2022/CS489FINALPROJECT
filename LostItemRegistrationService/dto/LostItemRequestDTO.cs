using LostItemRegistrationService.model;

namespace LostItemRegistrationService.dto
{
	public record LostItemRequestDTO
	{
	
		public IFormFile File { get; set; }	
		public string Description { get; set; }
		public string Category { get; set; }
		public DateTime DatetimeLost { get; set; }
		public string LocationLost { get; set; }
		public string status { get; set; }
	}
}
