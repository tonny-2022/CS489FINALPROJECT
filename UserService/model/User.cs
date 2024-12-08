namespace UserService.model
{
	public class User
	{
		public Guid UserId { get; set; }
		public  string UserName { get; set; }	
		public string Email { get; set; }
		public  string Password { get; set; }
		public bool IsAdmin { get; set; }
		public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
		public DateTimeOffset UpdatedAt { get; set; }=DateTimeOffset.UtcNow;

	}
}
