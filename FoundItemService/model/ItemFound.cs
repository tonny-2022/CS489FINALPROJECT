using System.ComponentModel.DataAnnotations;

namespace FoundItemService.model
{
	public class ItemFound
	{
		[Key]
		public Guid FoundId { get; set; }
		public string Description { get; set; }
		public DateTime DatetimeFound { get; set; }
		public string LocationFound { get; set; } 
		public string ImageUrl {  get; set; }
		public DateTime CreateAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; }= DateTime.UtcNow;
		public Guid UserId {  get; set; }
		public Guid ItemFoundId { get; internal set; }
		public string ItemDescription { get; internal set; }
	}
}
