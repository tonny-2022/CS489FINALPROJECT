using UserService.dto;
using UserService.model;

namespace UserService.repository
{
	public interface IUserRepository
	{
		Task<User> CreateUserAsync(User user);
		Task<User?> UpdateUserAsync(Guid userId ,User user);
		Task<User?> DeleteUserById(Guid userId);	
		Task<User?> GetUserById(Guid userId);
		Task<List<User?>> GetAllUsersAsync();

	}
}
