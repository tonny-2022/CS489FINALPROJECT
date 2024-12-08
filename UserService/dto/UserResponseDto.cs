namespace UserService.dto
{
	public record UserResponseDto(Guid UserId,string UserName,string Email,bool IsAdmin,
		                          DateTimeOffset CreatedAt,DateTimeOffset UpdatedAt){ }
	
}
