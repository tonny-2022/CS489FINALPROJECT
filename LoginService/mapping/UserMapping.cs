using AutoMapper;
using LoginService.dto;
using LoginService.model;

namespace LoginService.mapping
{
	public class UserMapping :Profile
	
	{
        public UserMapping()
        {
            CreateMap<ApplicationUser,UserResponseDTO>().ReverseMap();   
            CreateMap<ApplicationUser,UserRequestDTO>().ReverseMap();   
        }
    }
	
}
