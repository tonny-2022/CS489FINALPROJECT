using System.ComponentModel.DataAnnotations;

namespace LostItemUI.dto
{
	public class UserResponseDTO
	{
		[Required]
		public string Id{ get; set; }	
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Location { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		
		public string PhoneNumber { get; set; }
		public string []Roles { get; set; }
	}
}
