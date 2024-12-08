using LoginService.dto;
using LoginService.model;
using LoginService.repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LoginService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly ITokenRepository tokenRepository;

		public AuthController(UserManager<ApplicationUser> _userManager,ITokenRepository _tokenRepository)
        {
			this.userManager = _userManager;
			this.tokenRepository = _tokenRepository;
		}


		[HttpPost]
		//[Authorize(Roles = "Admin")]
		[Route("RegisterAdmin")]
		public async Task<IActionResult> RegisterAdmin([FromBody] AdminUserRequestDTO adminUserRequestDTO)
		{
			var existingUsername = await userManager.FindByEmailAsync(adminUserRequestDTO.UserName);
			var existingPhoneNumber = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == adminUserRequestDTO.PhoneNumber);

			if (existingUsername != null)
			{
				return BadRequest(new { Message = "Userame already exists" });
			}
			if (existingPhoneNumber != null) { return BadRequest(new { Message = "Phone alreaday exists" }); }

			var identityUser = new ApplicationUser() {
				FirstName = adminUserRequestDTO.FirstName,
				LastName = adminUserRequestDTO.LastName,
				Location = adminUserRequestDTO.Location,
				PhoneNumber = adminUserRequestDTO.PhoneNumber,
				UserName = adminUserRequestDTO.UserName,
				Email = adminUserRequestDTO.UserName,

			};
			// creating a user
			var identityResult = await userManager.CreateAsync(identityUser, adminUserRequestDTO.Password);
			if (identityResult.Succeeded)
			{
				// add roles to this user if the user has been succesfully created
				if (adminUserRequestDTO.Roles != null && adminUserRequestDTO.Roles.Any())
				{
					identityResult = await userManager.AddToRolesAsync(identityUser, adminUserRequestDTO.Roles);
					if (identityResult.Succeeded)
					{
						return Ok("Admin has been succesfully created");
					}
				}
			}
			return BadRequest(new
            {
                Message = "User failed to be created",
                Errors = identityResult.Errors.Select(e => e.Description)
            });


        }

 	   [HttpPost]
	 //  [Authorize(Roles ="Normal")]
	   [Route("RegisterNormalUser")]
		public async Task<IActionResult> RegisterNormalUser([FromBody] NormalUserRequestDTO normalUserRequestDTO)
		{
			var existingUsername = await userManager.FindByEmailAsync(normalUserRequestDTO.UserName);
			var existingPhoneNumber = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == normalUserRequestDTO.PhoneNumber);

			if (existingUsername != null)
			{
				return BadRequest(new { Message = "Userame already exists" });
			}
			if (existingPhoneNumber != null) { return BadRequest(new { Message = "Phone alreday exists" }); }
			var identityUser = new ApplicationUser()
			{
				FirstName = normalUserRequestDTO.FirstName,
				LastName = normalUserRequestDTO.LastName,
				UserName = normalUserRequestDTO.UserName,
				Location=normalUserRequestDTO.Location,
				Email = normalUserRequestDTO.UserName,
				PhoneNumber = normalUserRequestDTO.PhoneNumber
			};
			// creating a user
			var identityResult = await userManager.CreateAsync(identityUser, normalUserRequestDTO.Password);
			if (identityResult.Succeeded)
			{
				// add role to this user if the user has been succesfully created
				if (normalUserRequestDTO.Roles != null )
				{
					identityResult = await userManager.AddToRolesAsync(identityUser, normalUserRequestDTO.Roles);
					if (identityResult.Succeeded)
					{
						return Ok("User has been succesfully created");
					}
				}
			}
			return BadRequest(new
			{
				Message = "User failed to be created",
				Errors = identityResult.Errors.Select(e => e.Description)
			});
		}

		/*public  async Task<IActionResult?> ValidatePhoneNumberAndUserName(string username,string phoneNumber)
		{
			var existingUsername = await userManager.FindByEmailAsync(username);
			var existingPhoneNumber = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

			if (existingUsername != null)
			{
				return BadRequest(new { Message = "Userame already exists" });
			}
			if (existingPhoneNumber != null) { return BadRequest(new { Message = "Phone alreday exists" }); }
			return null;



		}*/

		[HttpPost("Login")]
		public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
		{
			
			// Admin user: "userName": "tonnyk@mail.com","password": "tonnyk@1234
			//Normal user :"userName": "hope@mail.com","password": "hope@1234"

			var existingUser = await userManager.FindByEmailAsync(loginRequestDTO.UserName);
			if (existingUser != null)
			{
				var isValidPassword= await userManager.CheckPasswordAsync(existingUser, loginRequestDTO.Password);
				if (isValidPassword)
				{
					//Get  Roles for this year
					var roles = await userManager.GetRolesAsync(existingUser);
					if (roles != null)
					{
						//Create a jwt token
						var jwtToken = tokenRepository.CreateJWTTokenForMutipleClaims(existingUser, roles.ToList());
						var response = new LoginResponseDTO
						{
							JwtToken = jwtToken,
						};


						return Ok(response);
					}
				}
			
			}
			return BadRequest("Username or Password is wrong");
		}
	}
}
