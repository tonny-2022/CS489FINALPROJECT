using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UserService.dto;
using UserService.model;
using UserService.repository;

namespace LostItemRegistrationService.Controllers
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
		public async Task<IActionResult> GetAll()
		{
			var users = await userRepository.GetAllUsersAsync();

			var userResponseDto = mapper.Map<List<UserResponseDto>>(users);
			return Ok(userResponseDto);
		}
		[HttpPost]

		public async Task<IActionResult> Create([FromBody] UserRequestDTO userRequestDTO)
		{
			if (ModelState.IsValid)
			{
				var userDomain = mapper.Map<User>(userRequestDTO);
				 await userRepository.CreateUserAsync(userDomain);
				//return CreatedAtAction(nameof(GetById), new {user.UserId},userDomain);
				var userDto=mapper.Map<UserResponseDto>(userDomain);
				return Ok(userDto);
			}
			else {
				return BadRequest(ModelState);
			}

		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] Guid id){
			var userDomain= await userRepository.GetUserById(id);

			if (userDomain is null)
			{
				return null;
			}

			return Ok(mapper.Map<UserResponseDto>(userDomain));

		}
		[HttpDelete("{id}")]
		//[Route("{id}")]
		public async Task<IActionResult> Delete([FromRoute] Guid id)
		{
			var userToDelete = await userRepository.DeleteUserById(id);
			if (userToDelete is null)
			{
				return NotFound();
			}
			return Ok (mapper.Map<UserResponseDto>(userToDelete));
		}

		[HttpPut]
		[Route("{id}")]
		public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UserRequestDTO userRequestDTO)
		{
			var userToUpdate=mapper.Map<User>(userRequestDTO);
			 await userRepository.UpdateUserAsync(id,userToUpdate);

			if (userToUpdate== null)
			{
				return NotFound(ModelState);
			}
			return Ok(mapper.Map<UserResponseDto>(userToUpdate));	
		}

    }
}
