using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LoginService.model
{
	public class ApplicationUser:IdentityUser
	{
		[Required]
		public string FirstName {  get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]	
		public string Location {  get; set; }
	}
}
