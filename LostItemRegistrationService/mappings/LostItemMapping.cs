
using AutoMapper;
using LostItemRegistrationService.dto;
using LostItemRegistrationService.model;

namespace LostItemRegistrationService.mappings
{
	public class LostItemMapping:Profile
	{
		public LostItemMapping()
		{
			CreateMap<LostItemRegistration, LostItemRequestDTO>().ReverseMap();

			CreateMap<LostItemRegistration, LostItemResponseDTO>().ReverseMap();
		
		}





	
	}
}
