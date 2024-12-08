using System.ComponentModel.DataAnnotations;

namespace LostItemRegistrationService.model
{
	public class LostItemRegistration
	{
		[Key]
		public Guid LostId { get; set; }	
		public string Description {  get; set; }	
		public string Category { get; set; }
		public DateTime DatetimeLost {  get; set; }
		public string LocationLost {  get; set; }	
		public string PhotoURL {  get; set; }	
		public string status {  get; set; }	
        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
		public DateTime UpdatedAt {  get; set; }= DateTime.UtcNow;	
		public string UserId {  get; set; }
	

		
	}
}
