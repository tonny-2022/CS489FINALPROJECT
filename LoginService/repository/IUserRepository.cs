using LoginService.model;

namespace LoginService.repository
{
	public interface IUserRepository
	{

		Task<List<ApplicationUser>> GetAllUsersAsyc();
		Task<ApplicationUser> GetUserByIdAsync(Guid userId);
		Task<ApplicationUser> UpdateUserAsync(Guid userId,ApplicationUser user);
		Task DeleteUserAsync(Guid userId);
	}
}
