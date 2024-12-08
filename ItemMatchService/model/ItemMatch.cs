using System.ComponentModel.DataAnnotations;

namespace ItemMatchService.model
{
	public class ItemMatch
	{
		[Key]
		public Guid MatchId {  get; set; }	
		public string ?MatchScore { get; set; }
		public string? Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set;}
		public Guid LostId { get; set; }
		public Guid FoundId { get; set; }
		public Guid UserId { get; set; }

	}


}
