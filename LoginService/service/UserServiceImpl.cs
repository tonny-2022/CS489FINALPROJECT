using LoginService.data;
using LoginService.model;
using LoginService.repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;

namespace LoginService.service
{
	public class UserServiceImpl : IUserRepository
	{
		private readonly AuthDbContext authDbContext;
        public UserServiceImpl(AuthDbContext _authDbContext)
        {
            this.authDbContext = _authDbContext;	
        }

		public async Task DeleteUserAsync(Guid userId)
		{
			var userToDelete = await GetUserByIdAsync(userId); 
			if (userToDelete != null)
			{
				authDbContext.Users.Remove(userToDelete);
				await authDbContext.SaveChangesAsync();	
			}
		}

		public async Task<List<ApplicationUser>> GetAllUsersAsyc()
		{
			return await authDbContext.Users.ToListAsync();
		}

		public async Task<ApplicationUser> GetUserByIdAsync(Guid userId)
		{
			var user= await authDbContext.Users.FirstOrDefaultAsync(user=>user.Id==userId.ToString());
			if(user==null)
			{
				return null;
			}
			return user;
		}

		public async Task<ApplicationUser> UpdateUserAsync(Guid userId, ApplicationUser user)
		{
			var userToUpddate= await GetUserByIdAsync(userId);

			if(userToUpddate!=null)
			{
				userToUpddate.FirstName = user.FirstName;	
				userToUpddate.LastName = user.LastName;
				userToUpddate.Location= user.Location;
				userToUpddate.PhoneNumber= user.PhoneNumber;
				userToUpddate.PasswordHash= user.PasswordHash;
				authDbContext.Entry(userToUpddate).State=EntityState.Modified;// Mark entity as modified
				await authDbContext.SaveChangesAsync();
				return user;
			}
			return null;
		}
	}
}
