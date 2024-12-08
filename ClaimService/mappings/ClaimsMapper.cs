using System.Security.Claims;
using AutoMapper;
using ClaimService.dto;
using ClaimService.model;

namespace ClaimService.mappings
{
	public class ClaimsMapper:Profile
	{
		public ClaimsMapper() {


			CreateMap<ItemClaim,ItemClaimRequestDTO>().ReverseMap();
			CreateMap<ItemClaim, ItemClaimResponseDTO>()
			.ConstructUsing(src => new ItemClaimResponseDTO(
				src.ItemClaimID,
				src.VerificationDetails,
				src.Status,
				src.CreatedAt,
				src.UpdatedAt,
				src.MatchId
				)).ReverseMap();

		}
	}
}
