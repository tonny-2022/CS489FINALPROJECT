using LoginService.model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LoginService.data
{
	public class AuthDbContext : IdentityDbContext<ApplicationUser>
	{
		public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
		{


		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			var normalRoleId = "bdbad895-93b5-4d45-a070-db5c3d97dc3c";
			var adminRoleId = "674bad9b-cfd8-4a67-b79d-3f12a9bf3e64";
			var roles = new List<IdentityRole> { 
			 new IdentityRole
			 {
				 Id = normalRoleId,
				 ConcurrencyStamp =normalRoleId,
				 Name = "Normal",
				 NormalizedName ="Normal".ToUpper(),
			 },

			 new IdentityRole
			 {
				 Id = adminRoleId,
				 ConcurrencyStamp =adminRoleId,
				 Name = "Admin",
				 NormalizedName ="Admin".ToUpper(),
			 }

			};
			builder.Entity<IdentityRole>().HasData(roles);
		}
	}
}
