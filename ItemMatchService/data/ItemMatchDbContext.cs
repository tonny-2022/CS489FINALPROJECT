using ItemMatchService.model;
using Microsoft.EntityFrameworkCore;

namespace ItemMatchService.data
{
	public class ItemMatchDbContext:DbContext
	{
        public ItemMatchDbContext(DbContextOptions<ItemMatchDbContext> options):base(options) 
        {
           // Database.EnsureCreated();
        }
        public DbSet<ItemMatch> ItemsMatch  { get; set; }
    }
}
