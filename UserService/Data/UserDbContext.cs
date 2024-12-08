using Microsoft.EntityFrameworkCore;
using UserService.model;

namespace UserService.Data
{
	public class UserDbContext:DbContext
	{
        public UserDbContext(DbContextOptions<UserDbContext> options) :base(options) 
        {
            Database.EnsureCreated();
        }

		public DbSet<User> Users { get; set; }	
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
		  modelBuilder.Entity<User>().HasData(
				new User
				{
					UserId = Guid.NewGuid(),
					UserName = "jdoe",
					Email = "jdoe@gmail.com",
					Password = "password123",  // Normally, passwords should be hashed and not stored as plain text
					IsAdmin = false,
					CreatedAt = new DateTime(2023, 8, 1, 10, 30, 0),
					UpdatedAt = new DateTime(2023, 8, 15, 11, 45, 0)
				});
			modelBuilder.Entity<User>().HasData(new User
			{
				UserId = Guid.NewGuid(),
				UserName = "asmith",
				Email = "asmith@yahoo.com",
				Password = "securePass!23",  // Placeholder for demonstration
				IsAdmin = true,
				CreatedAt = new DateTime(2022, 12, 5, 9, 0, 0),
				UpdatedAt = new DateTime(2023, 2, 12, 15, 20, 0)
			});
			modelBuilder.Entity<User>().HasData(new User
			{
				UserId = Guid.NewGuid(),
				UserName = "mwhite",
				Email = "mwhite@gmail.com",
				Password = "White@45",  // Placeholder for demonstration
				IsAdmin = false,
				CreatedAt = new DateTime(2021, 6, 21, 8, 15, 0),
				UpdatedAt = new DateTime(2023, 1, 10, 14, 0, 0)
			});
			modelBuilder.Entity<User>().HasData(new User
	{
		UserId = Guid.NewGuid(),
		UserName = "ljones",
		Email = "ljones@hotmail.com",
		Password = "Jones$789",  // Placeholder for demonstration
		IsAdmin = true,
		CreatedAt = new DateTime(2020, 4, 30, 17, 30, 0),
	    UpdatedAt = new DateTime(2023, 10, 9, 19, 45, 0)
	    });
			base.OnModelCreating(modelBuilder);
		}
	}
}
