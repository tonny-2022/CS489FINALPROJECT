using AutoMapper;
using LoginService.dto;
using LoginService.model;
using LoginService.repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoginService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]

	
	public class UserController : ControllerBase
	{
		private readonly IUserRepository userRepository;
		private readonly IMapper mapper;

		public UserController(IUserRepository _userRepository,IMapper _mapper)
        {
			userRepository = _userRepository;
			this.mapper = _mapper;
		}
		[HttpGet]
		public async Task< IActionResult> GetAll() {
			
			var users= await userRepository.GetAllUsersAsyc();
			var usersDto =  mapper.Map<List<UserResponseDTO>>(users);

			
			if (users==null)
			{
				return null;
			}
			return Ok(usersDto);	
		}

		[HttpGet("{userId}")]
		public async Task<IActionResult> GetById(Guid userId) {

			var user = await userRepository.GetUserByIdAsync(userId);
			var userDto=mapper.Map<UserResponseDTO>(user);
			return user==null?NotFound(): Ok(userDto);	

		}

		[HttpDelete("{userId}")]
		public async Task<IActionResult>Delete([FromRoute] Guid userId)
		{
			await userRepository.DeleteUserAsync(userId);
			return NoContent();
		}
		[HttpPut("{userId}")]
		public async Task<IActionResult> Update([FromRoute] Guid userId, [FromBody] UserRequestDTO requestDTO)
		{
			var user= mapper.Map<ApplicationUser>(requestDTO);	
			await userRepository.UpdateUserAsync(userId, user);

			return user==null ? NotFound(ModelState) : Ok(mapper.Map<UserResponseDTO>(user));

		}
    }
}
