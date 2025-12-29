namespace JordanTouristGuide.Models
{
    public class Hotel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
        public double Rating { get; set; }
        public List<string> Amenities { get; set; } = new List<string>();
        public string Address { get; set; } = string.Empty;
    }
}
