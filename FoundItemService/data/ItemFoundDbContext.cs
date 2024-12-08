using FoundItemService.model;
using Microsoft.EntityFrameworkCore;

namespace FoundItemService.data
{
	public class ItemFoundDbContext:DbContext
	{
        public ItemFoundDbContext(DbContextOptions<ItemFoundDbContext> options):base(options) 
        {
            Database.EnsureCreated();
        }
        public DbSet<ItemFound> ItemsFound { get; set; } 
    }
}
