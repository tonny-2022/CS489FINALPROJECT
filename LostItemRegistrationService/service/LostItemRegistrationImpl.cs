using System.Security.Claims;
using LostItemRegistrationService.Data;
using LostItemRegistrationService.model;
using LostItemRegistrationService.repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace LostItemRegistrationService.service
{
	public class LostItemRegistrationImpl : ILostItemRegistrationRepository,IMageUploadRepository
	{
		ItemReportDbContext itemReportDbContext;
		private readonly IWebHostEnvironment hostEnvironment;
		private readonly IHttpContextAccessor httpContextAccessor;

		


		public LostItemRegistrationImpl(ItemReportDbContext _itemReportDbContext, 
			                            IWebHostEnvironment _hostEnvironment,
										IHttpContextAccessor _httpContextAccessor)
        {
            itemReportDbContext = _itemReportDbContext;
			hostEnvironment = _hostEnvironment;
			httpContextAccessor = _httpContextAccessor;	
        }

		public string? GetUserId()
		{
			return httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		}
		public async Task<LostItemRegistration> CreateAsync(LostItemRegistration lostItem)
		{
			var userId=GetUserId();
			if (userId==null)
			{
       		throw new UnauthorizedAccessException("User Id not found in token.");
			}
			lostItem.UserId = userId;
			await itemReportDbContext.LostItems.AddAsync(lostItem);
			await itemReportDbContext.SaveChangesAsync();
			return lostItem;
		}

		public async Task <LostItemRegistration>DeleteAsync(Guid itemId)
		{
			var itemToDelete=await GetByIdAsync(itemId);
			if (itemToDelete != null)
			{
				 itemReportDbContext.LostItems.Remove(itemToDelete);
				await itemReportDbContext.SaveChangesAsync();
				return itemToDelete;
			}
			return null;
		}

		public async Task<List<LostItemRegistration>> GetAllAsync()
		{
			return await itemReportDbContext.LostItems.ToListAsync();
		}

		public Task<LostItemRegistration> GetByIdAsync(Guid itemId)
		{
			var item = itemReportDbContext.LostItems.FirstOrDefaultAsync(it=>it.LostId== itemId);
			if (item==null)
			{
				return null;
			}
			return item;
		}

		public async Task<LostItemRegistration?> UpdateAsync(Guid itemId, LostItemRegistration lostItem)
		{
			var itemToUdate = await GetByIdAsync(itemId);
			if (itemToUdate != null)
			{
				itemToUdate.Description = lostItem.Description;
				itemToUdate.Category = lostItem.Category;
				itemToUdate.DatetimeLost = lostItem.DatetimeLost;
				itemToUdate.LocationLost = lostItem.LocationLost;
				itemToUdate.PhotoURL = lostItem.PhotoURL;
				itemToUdate.status = lostItem.status;
				await itemReportDbContext.SaveChangesAsync();

				return lostItem;
			}
			return null;
	}

		public async Task<List<LostItemRegistration>> GetAllByUserAsync()
		{
			var userId = GetUserId();
			return await itemReportDbContext.LostItems.Where(item=>item.UserId==userId).ToListAsync();
		}

		public  async Task<Image> UploadImage(Image image)
		{
			var localPath = Path.Combine(hostEnvironment.ContentRootPath, "images",
										 $"{image.FileName}{image.FileExtension}");
			//  Upload image to the folder
			var stream = new FileStream(localPath, FileMode.Create);
			await image.File.CopyToAsync(stream);

			var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/images/{image.FileName}{image.FileExtension}";
			image.FilePath = urlFilePath;
			return image;
		}

		public  async Task<string> UpdateStatus(Guid lostId, string status)
		{
			var lostItem = await itemReportDbContext.LostItems.FirstOrDefaultAsync(item => item.LostId == lostId);
			if (lostItem == null)
			{
				throw new KeyNotFoundException($"Lost item with ID {lostId} not found.");
			}
			lostItem.status = status;
			await itemReportDbContext.SaveChangesAsync();

			return lostItem.status;
		}
	}
}
