namespace ClaimService.dto
{
	public record ItemClaimRequestDTO(
		string VerificationDetails,
		string Status,
		Guid MatchId

		);
	
}
