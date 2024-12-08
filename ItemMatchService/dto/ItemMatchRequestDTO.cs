namespace ItemMatchService.dto
{
	public record ItemMatchRequestDTO
	{
		public string? MatchScore { get; set; }
		public string? Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public Guid LostId { get; set; } 
		public Guid FoundId { get; set; }

	}
}
