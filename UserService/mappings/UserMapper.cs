using AutoMapper;
using UserService.dto;
using UserService.model;

namespace UserService.mappings
{
	public class UserMapper:Profile
	{
		public UserMapper() {
			CreateMap<User,UserRequestDTO>().ReverseMap();
			CreateMap<User,UserResponseDto>().ReverseMap();
		}
	}
}
