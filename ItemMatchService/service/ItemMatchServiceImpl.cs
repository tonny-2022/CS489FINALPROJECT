using System.Net.Http;
using System.Security.Claims;
using ItemMatchService.data;
using ItemMatchService.dto;
using ItemMatchService.model;
using ItemMatchService.repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;

namespace ItemMatchService.service
{
	public class ItemMatchServiceImpl : ItemMatchRepository
	{
		private readonly ItemMatchDbContext itemMatchDbContext;
		private readonly HttpClient httpClient;
		private readonly IHttpContextAccessor httpContextAccessor;

		public ItemMatchServiceImpl(ItemMatchDbContext _itemMatchDbContext,
			 HttpClient _httpClient,IHttpContextAccessor _httpContextAccessor) {

			this.itemMatchDbContext = _itemMatchDbContext;	
			this.httpClient = _httpClient;
			httpContextAccessor = _httpContextAccessor;
		}

		public async  Task<ItemMatch> CreateAsync(ItemMatch itemMatch)
		{
			//getting userId from JWT  that has been passed through claims by the Login service after logging in the system

			var userId =  httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId==null)
			{
				throw new UnauthorizedAccessException("User Id not found in token.");
			}

			var foundItemResponse = await httpClient.GetAsync($"https://localhost:7216/api/itemsfound/{itemMatch.FoundId}");
			if (!foundItemResponse.IsSuccessStatusCode)
			{
				throw new ($"Item with Id {itemMatch.FoundId} not found.");
			}
			var foundItem=await foundItemResponse.Content.ReadFromJsonAsync<ItemFoundResponseDTO>();	

			var lostItemResponse = await httpClient.GetAsync($"https://localhost:7008/api/lostitem/{itemMatch.LostId}");
			if (!lostItemResponse.IsSuccessStatusCode)
			{
				throw new Exception($"Lost item with ID {itemMatch.LostId} not found.");
			}
			var lostItem = await lostItemResponse.Content.ReadFromJsonAsync<ItemLostResponseDTO>();

			itemMatch.FoundId = foundItem.FoundId;
			itemMatch.LostId = lostItem.LostId;
			itemMatch.UserId = Guid.Parse(userId);
			await itemMatchDbContext.ItemsMatch.AddAsync(itemMatch);
			await itemMatchDbContext.SaveChangesAsync();

			// update the status of lost item  status column with matching id to matched
			var updateLostItemResponse = await httpClient.PatchAsJsonAsync(
				$"https://localhost:7008/api/lostitem/{lostItem.LostId}/status",new { Status="matched" });

			if (!updateLostItemResponse.IsSuccessStatusCode)
			{
				var errorContent = await updateLostItemResponse.Content.ReadAsStringAsync();
				throw new Exception($"Failed to update the status of the lost item to 'matched'. Status Code: {updateLostItemResponse.StatusCode}, Error: {errorContent}");
			}
			return itemMatch;
		}
		public async Task<ItemMatch> GetByIdAsync(Guid matchId)
		{
			var itemToMatch=await itemMatchDbContext.ItemsMatch.FirstOrDefaultAsync(it=>it.MatchId==matchId);
			if (itemToMatch==null)
			{
				return null;
			}
			return itemToMatch;
		}

		public async Task DeleteAsync(Guid matchId)
		{
			var itemToDelete= await GetByIdAsync(matchId);
			if (itemToDelete!=null)
			{
				itemMatchDbContext.ItemsMatch.Remove(itemToDelete);
				await itemMatchDbContext.SaveChangesAsync();
			}
		}

		public async Task<List<ItemMatch>> GetAllAsync()
		{
			return await itemMatchDbContext.ItemsMatch.ToListAsync();
		}

	

		public async  Task<ItemMatch> UpdateAsync(Guid matchId, ItemMatch itemMatch)
		{
			var matchToUpdate = await GetByIdAsync(matchId);

			if (matchToUpdate!=null)
			{
				matchToUpdate.Status = itemMatch.Status;
				matchToUpdate.MatchScore = itemMatch.MatchScore;
				matchToUpdate.UpdatedAt = DateTime.UtcNow;	
				await itemMatchDbContext.SaveChangesAsync();
				return itemMatch;
			}
			return null;
		}
	}
}
