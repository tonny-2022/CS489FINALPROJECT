using System.Security.Claims;
using ClaimService.model;

namespace ClaimService.repository
{
	public interface IClaimRepository
	{
		Task<List<ItemClaim>> GetAllClaimsAysnc();	
		Task<ItemClaim> CreateClaimAsync(ItemClaim claim);	
		Task<ItemClaim> UpdateClaimAsync(Guid claimId,ItemClaim claim);	
		Task<ItemClaim> DeleteClaimAsync(Guid claimId);
		Task<ItemClaim> GetClaimByIdAsync(Guid claimId);



		

	}
}
