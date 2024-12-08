
using System.Security.Claims;
using System.Text.Json;
using AutoMapper;
using ClaimService.Data;
using ClaimService.dto;
using ClaimService.model;
using ClaimService.repository;
using Microsoft.EntityFrameworkCore;

namespace ClaimService.service
{
	public class ClaimRepositoryImpl(ClaimDbContext _claimDbContext, HttpClient _httpClient,IHttpContextAccessor _httpContextAccessor) : IClaimRepository
	{
		private readonly ClaimDbContext claimDbContext = _claimDbContext;
		private readonly HttpClient httpClient = _httpClient;
		private readonly IHttpContextAccessor httpContextAccessor = _httpContextAccessor;

		public async Task<ItemClaim> CreateClaimAsync(ItemClaim claim)
		{
			if (claim.MatchId == Guid.Empty)
			{
				throw new ArgumentException("MatchId is invalid.");
			}
			var itemMatchResponse = await httpClient.GetAsync($"https://localhost:7192/api/ItemMatch/{claim.MatchId}");
			if (!itemMatchResponse.IsSuccessStatusCode)
			{
				var errorContent = await itemMatchResponse.Content.ReadAsStringAsync();
				throw new Exception($"Failed to retrieve item match. Status Code: {itemMatchResponse.StatusCode}, Error: {errorContent}");
			}
			// Read response content
			var responseContent = await itemMatchResponse.Content.ReadAsStringAsync();
			// Deserialize with case-insensitivity
			ItemMatchResponseDTO itemMatch;
			try
			{
			  itemMatch = JsonSerializer.Deserialize<ItemMatchResponseDTO>(
		      responseContent,
			  new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
	         );
			}
			catch (JsonException ex)
			{
				throw new Exception($"Error deserializing JSON response: {ex.Message}\nResponse Content: {responseContent}", ex);
			}

			
			if (itemMatch == null || itemMatch.LostId == Guid.Empty)
			{
				throw new ArgumentException($"Invalid data from ItemMatch API. Deserialized itemMatch: {JsonSerializer.Serialize(itemMatch, new JsonSerializerOptions { WriteIndented = true })}");
			}

			var lostId = itemMatch.LostId;

			var updateLostItemResponse = await httpClient.PatchAsJsonAsync(
				$"https://localhost:7008/api/lostitem/{lostId}/status", new { Status = "claimed" });

			if (!updateLostItemResponse.IsSuccessStatusCode)
			{
				var errorContent = await updateLostItemResponse.Content.ReadAsStringAsync();
				throw new Exception($"Failed to update the status of the lost item to 'claimed'. Status Code: {updateLostItemResponse.StatusCode}, Error: {errorContent}");
			}
			var userId = httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
			{
				throw new UnauthorizedAccessException("User Id not found in token.");
			}
		
			claim.UserId = Guid.Parse(userId);
			await claimDbContext.ItemClaims.AddAsync(claim);
			await claimDbContext.SaveChangesAsync();
			return claim;
		}

		public async Task<ItemClaim> DeleteClaimAsync(Guid claimId)
		{
			
			var claimToDelete = await GetClaimByIdAsync(claimId);
			if (claimToDelete!=null)
			{
				claimDbContext.ItemClaims.Remove(claimToDelete);
				await claimDbContext.SaveChangesAsync();
				return claimToDelete;
			}
			return null;
		}

		public async Task<List<ItemClaim>> GetAllClaimsAysnc()
		{
			return await claimDbContext.ItemClaims.ToListAsync();
		}

		public async Task<ItemClaim> GetClaimByIdAsync(Guid claimId)
		{
			var claim= await claimDbContext.ItemClaims.FirstOrDefaultAsync(It=>It.ItemClaimID==claimId);
			if (claim==null)
			{
				return null;
			}
			return claim;
		}

		public async Task<ItemClaim> UpdateClaimAsync(Guid claimId, ItemClaim claim)
		{
			var claimToUpdate= await GetClaimByIdAsync(claimId);
			if (claimToUpdate!=null)
			{				
				claimToUpdate.VerificationDetails= claim.VerificationDetails;
				claimToUpdate.Status= claim.Status;	
				claimToUpdate.UpdatedAt= DateTime.UtcNow;	
				await claimDbContext.SaveChangesAsync();
				return claim;
			}
			return null;
		}
	}
}
