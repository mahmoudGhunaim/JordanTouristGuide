using System.ComponentModel.DataAnnotations;

namespace JordanTouristGuide.Models
{
    public class Property
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        [Range(0, 100000)]
        public decimal PricePerNight { get; set; }

        [Range(0, 5)]
        public double Rating { get; set; }

        public string Amenities { get; set; } = string.Empty;

        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? CreatedByUserId { get; set; }
        public ApplicationUser? CreatedByUser { get; set; }
    }
}
