using AutoMapper;
using FoundItemService.dto;
using FoundItemService.model;

namespace FoundItemService.mapping
{
	public class ItemFoundMapping:Profile
	{
        public ItemFoundMapping()
        {
            CreateMap<ItemFound, ItemFoundRequestDTO>().ReverseMap();
            CreateMap<ItemFound,ItemFoundResponseDTO>().ReverseMap();
        }
    }
}
