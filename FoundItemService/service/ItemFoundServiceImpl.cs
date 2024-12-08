using System.Security.Claims;
using FoundItemService.data;
using FoundItemService.model;
using FoundItemService.repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace FoundItemService.service
{
	public class ItemFoundServiceImpl : ItemFoundRepository
	{

		private readonly ItemFoundDbContext itemFoundDbContext;
		private readonly IWebHostEnvironment hostEnvironment;
		private readonly IHttpContextAccessor httpContextAccessor;
		public ItemFoundServiceImpl(ItemFoundDbContext itemFoundDbContext,
			                        IWebHostEnvironment _hostEnvironment,
									IHttpContextAccessor _httpContextAccessor 
			) { 
			this.itemFoundDbContext = itemFoundDbContext;	
			this.hostEnvironment = _hostEnvironment;
			this.httpContextAccessor=_httpContextAccessor;
		}

		public async Task<ItemFound> CreateItemFoundAsync(ItemFound itemFound)
		{
			var userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId==null)
			{
				throw new UnauthorizedAccessException("User Id not found in token.");
			}
			itemFound.UserId = Guid.Parse(userId);
			await itemFoundDbContext.ItemsFound.AddAsync(itemFound);
			await itemFoundDbContext.SaveChangesAsync();	
			return itemFound;
		}

		public async Task DeleteItemFoundAsync(Guid itemFoundId)
		{
			var itemToDelete= await GetItemFoundByIdAsync(itemFoundId);
			if (itemToDelete != null)
			{
				itemFoundDbContext.ItemsFound.Remove(itemToDelete);
				await itemFoundDbContext.SaveChangesAsync();
			}	
		}

		public async Task<IEnumerable<ItemFound>> GetAllItemFoundAsync()
		{
			return await itemFoundDbContext.ItemsFound.ToListAsync();
		}

		public async Task<ItemFound> GetItemFoundByIdAsync(Guid itemFoundId)
		{
			var itemFound = await itemFoundDbContext.ItemsFound.FirstOrDefaultAsync(it => it.FoundId == itemFoundId);
			if (itemFound==null)
			{
				return null;
			}
			return itemFound;
		}

		public async Task<ItemFound> UpdateItemFoundAsync(Guid itemFoundId, ItemFound itemFound)
		{
			var itemToUpdate= await GetItemFoundByIdAsync(itemFoundId);	
			if(itemToUpdate != null) { 

				itemToUpdate.DatetimeFound = itemFound.DatetimeFound;
				itemToUpdate.LocationFound= itemFound.LocationFound;	
				itemToUpdate.Description=itemFound.Description;
				itemToUpdate.UpdatedAt=itemFound.UpdatedAt;
				await itemFoundDbContext.SaveChangesAsync();
				return  itemToUpdate;
			}
			return null;
		}

		public async Task<Image> UploadImage(Image image)
		{
			var localPath = Path.Combine(hostEnvironment.ContentRootPath, "images",
										 $"{image.FileName}{image.FileExtension}");
			//Upload image to the folder
			var stream = new FileStream(localPath, FileMode.Create);
			await image.File.CopyToAsync(stream);

			var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/images/{image.FileName}{image.FileExtension}";
			image.FilePath = urlFilePath;
			return image;
		}
	}
}
