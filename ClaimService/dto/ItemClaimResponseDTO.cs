namespace ClaimService.dto
{
	public record ItemClaimResponseDTO(
	    Guid ClaimID,
		string VerificationDetails,
		string Status,
		DateTime CreatedAt,
	    DateTime UpdatedAt,
		Guid MatchId
		);
}
