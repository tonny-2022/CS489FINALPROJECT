using LostItemRegistrationService.dto;
using LostItemRegistrationService.model;
using Microsoft.AspNetCore.Mvc;

namespace LostItemRegistrationService.repository
{
	public interface ILostItemRegistrationRepository
	{
		Task<LostItemRegistration> CreateAsync(LostItemRegistration lostItem);
		Task<LostItemRegistration?> UpdateAsync(Guid itemId, LostItemRegistration lostItem);
		Task<LostItemRegistration> DeleteAsync(Guid itemId);
		Task<LostItemRegistration> GetByIdAsync(Guid itemId);
		Task<List<LostItemRegistration>> GetAllAsync();
		Task<List<LostItemRegistration>> GetAllByUserAsync();
		Task<string> UpdateStatus(Guid lostId, string status);

	}
}