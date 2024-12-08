namespace ItemMatchService.dto
{
	public class ItemFoundResponseDTO
	{
		public Guid FoundId { get; set; }
		public string? Description { get; set; }
		public DateTime DatetimeFound { get; set; }
		public string ?LocationFound { get; set; }
		public string ?ImageUrl { get; set; }
		public DateTime CreateAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public Guid UserId { get; set; }
	}
}
