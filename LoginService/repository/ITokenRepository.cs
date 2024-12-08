using Microsoft.AspNetCore.Identity;

namespace LoginService.repository
{
	public interface ITokenRepository
	{
		public string CreateJWTTokenForMutipleClaims(IdentityUser identityUser, List<string> roles);
		public string CreateJWTTokenForSingleClaim(IdentityUser identityUser, List<string> roles);
	}
}