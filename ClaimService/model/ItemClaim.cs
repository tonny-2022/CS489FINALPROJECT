using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimService.model
{
	public class ItemClaim
	{
		[Key]
		public Guid ItemClaimID { get; set; }
		public string? VerificationDetails { get; set; }
		public string? Status { get; set; }
		public DateTime CreatedAt { get; set; }= DateTime.UtcNow;	
		public DateTime UpdatedAt { get; set;} = DateTime.UtcNow;
		//this is logical reference to ItemMatch service,no need to add [Foreignkey] atrribute
		public Guid MatchId { get; set; }
		public Guid UserId { get; set; }

	}
}
