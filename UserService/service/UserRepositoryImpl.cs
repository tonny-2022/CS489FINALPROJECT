using System.Runtime.ConstrainedExecution;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UserService.Data;
using UserService.dto;
using UserService.model;
using UserService.repository;

namespace UserService.service
{
	public class UserRepositoryImpl : IUserRepository
	{
		private readonly UserDbContext userDbContext;
        public UserRepositoryImpl(UserDbContext _userDbContex)
        {userDbContext = _userDbContex;
            
        }

		public  async Task<User> CreateUserAsync(User user)
		{
			await userDbContext.Users.AddAsync(user);
			await userDbContext.SaveChangesAsync();
			return user;
			
		}

	
		public  async Task<User?> GetUserById(Guid userId)
		{
			var user =await userDbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
			if (user == null)
			{
				return null;	
			}
			return user;
		}

		
		public async Task<List<User>> GetAllUsersAsync()
			
		{
	     	return await  userDbContext.Users.ToListAsync(); 

		}

		public async Task<User?> UpdateUserAsync(Guid userId,User user)
		{
			var userToDelete= await GetUserById(userId);

			if (userToDelete is null)
			{
				return null;
			}
		    user.UserName = userToDelete.UserName;
			user.Password=userToDelete.Password;
			user.Email=userToDelete.Email=user.Email;
			user.IsAdmin=userToDelete.IsAdmin;
			await userDbContext.SaveChangesAsync();
			return userToDelete ;	
		}

		public  async Task<User?> DeleteUserById(Guid userId)
		{
			var userToDelete = await GetUserById(userId);
			if (userToDelete == null)
			{
				return null;
			}
			userDbContext.Users.Remove(userToDelete);
			await userDbContext.SaveChangesAsync();
			return userToDelete;
		}
	}
}
