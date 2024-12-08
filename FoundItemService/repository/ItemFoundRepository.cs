using FoundItemService.model;

namespace FoundItemService.repository
{
	public interface ItemFoundRepository
	{
		Task<IEnumerable<ItemFound>> GetAllItemFoundAsync();
		Task<ItemFound> CreateItemFoundAsync(ItemFound itemFound);
		Task<ItemFound> UpdateItemFoundAsync(Guid itemFoundId,ItemFound itemFound);
		Task DeleteItemFoundAsync(Guid itemFoundId);
		Task<ItemFound> GetItemFoundByIdAsync(Guid itemFoundId);
		Task<Image> UploadImage(Image image);


	}
}
