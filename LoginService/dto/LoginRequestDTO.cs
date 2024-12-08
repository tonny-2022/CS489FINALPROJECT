﻿using System.ComponentModel.DataAnnotations;

namespace LoginService.dto
{
	public class LoginRequestDTO
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		public string UserName { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	
	}
}
