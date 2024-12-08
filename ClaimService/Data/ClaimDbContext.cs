using ClaimService.model;
using Microsoft.EntityFrameworkCore;

namespace ClaimService.Data
{
	public class ClaimDbContext:DbContext
	{
        public ClaimDbContext(DbContextOptions<ClaimDbContext> claimDbContext):base(claimDbContext)
        {
           // Database.EnsureCreated();
            
        }
        public DbSet<ItemClaim> ItemClaims { get; set; }

    }
}
