using LostItemRegistrationService.model;
using Microsoft.EntityFrameworkCore;

namespace LostItemRegistrationService.Data
{
	public class ItemReportDbContext:DbContext
	{    
        public ItemReportDbContext(DbContextOptions<ItemReportDbContext> options):base(options) 
        {
            Database.EnsureCreated();
        }
        public DbSet<LostItemRegistration> LostItems { get; set; }
    }
}
