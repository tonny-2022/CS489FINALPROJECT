using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginService.repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace LoginService.service
{
	public class TokenServiceImpl : ITokenRepository
	{
		private readonly IConfiguration configuration;

		public TokenServiceImpl(IConfiguration _configuration)
        {
			this.configuration = _configuration;
		}
        public string CreateJWTTokenForMutipleClaims(IdentityUser identityUser, List<string> roles)
		{
			var claims= new List<Claim>();
			
			   claims.Add(new Claim(ClaimTypes.Email,identityUser.Email));
			   claims.Add(new Claim(ClaimTypes.NameIdentifier, identityUser.Id));

			foreach(var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

				
			var audiences = configuration.GetSection("Jwt:Audiences").Get<string[]>();
			foreach (var audince in audiences)
			{
				if (audiences!=null) {
					claims.Add( new Claim(JwtRegisteredClaimNames.Aud,audince));
				}
			}

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
			var credentials= new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
			issuer: configuration["Jwt:Issuer"],
			claims: claims,
			expires: DateTime.Now.AddHours(24),
			signingCredentials: credentials
	        );
			return new JwtSecurityTokenHandler().WriteToken(token);	
			
		}

		public string CreateJWTTokenForSingleClaim(IdentityUser identityUser, List<string> roles)
		{
			var claims = new List<Claim>();
			claims.Add(new Claim(ClaimTypes.Email, identityUser.Email));

			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				configuration["Jwt:Issuer"],
				configuration["Jwt:Audience"],
				claims,
				expires: DateTime.Now.AddMinutes(30),
				signingCredentials: credentials

				);


			return new JwtSecurityTokenHandler().WriteToken(token);

		}
	}
}
