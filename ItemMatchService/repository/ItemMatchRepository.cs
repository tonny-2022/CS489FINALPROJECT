using ItemMatchService.model;

namespace ItemMatchService.repository
{
	public interface ItemMatchRepository
	{
		Task<ItemMatch> CreateAsync(ItemMatch itemMatch);
		Task<ItemMatch> UpdateAsync(Guid matchId, ItemMatch itemMatch);
		Task<List<ItemMatch>> GetAllAsync();
		Task DeleteAsync(Guid matchId);
		Task<ItemMatch> GetByIdAsync(Guid matchId);

	}
}
