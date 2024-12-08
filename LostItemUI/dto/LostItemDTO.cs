namespace LostItemUI.dto
{
    public class LostItemDTO
    {
        public Guid LostId { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime DatetimeLost { get; set; }
        public string LocationLost { get; set; }
        public string PhotoURL { get; set; }
        public string status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
