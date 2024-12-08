namespace FoundItemService.dto
{
	public record ItemFoundRequestDTO
	{
		public IFormFile File { get; set; }
		public string Description { get; set; }
		public DateTime DatetimeFound { get; set; }
		public string LocationFound { get; set; }

	}
}
