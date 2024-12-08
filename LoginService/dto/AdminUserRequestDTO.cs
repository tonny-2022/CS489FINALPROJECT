using System.ComponentModel.DataAnnotations;

namespace LoginService.dto
{
	public class AdminUserRequestDTO
	{
		[Required]
		public string FirstName { get; set; }
		[Required]	
		
		public string LastName { get; set; }
		[Required]	
		public string Location { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		public string UserName { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required]

		[DataType(DataType.PhoneNumber)]
		public string PhoneNumber { get; set; }
		[Required]
		public string [] Roles { get; set; }
	}
}
