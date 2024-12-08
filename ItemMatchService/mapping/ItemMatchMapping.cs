using AutoMapper;
using ItemMatchService.dto;
using ItemMatchService.model;

namespace ItemMatchService.mapping
{
	public class ItemMatchMapping:Profile
	{	
		public 	ItemMatchMapping() {
			CreateMap<ItemMatch, ItemMatchResponseDTO>();
			CreateMap<ItemMatchRequestDTO, ItemMatch>();
		}
		
		}
}
