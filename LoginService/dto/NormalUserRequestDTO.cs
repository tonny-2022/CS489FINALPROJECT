using System.ComponentModel.DataAnnotations;

namespace LoginService.dto
{
	public class NormalUserRequestDTO
	{
		public string FirstName {  get; set; }
		public string LastName { get; set; }
		public string Location {  get; set; }	

		[Required]
		[DataType(DataType.EmailAddress)]
		public string UserName { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required]

		[DataType(DataType.PhoneNumber)]	
		public string PhoneNumber {  get; set; }
		public string [] Roles { get; set; }
	}
}
