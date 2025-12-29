using System.ComponentModel.DataAnnotations;

namespace JordanTouristGuide.Models
{
    public class ExperienceDb
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        [StringLength(50)]
        public string Duration { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        public decimal? Price { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? CreatedByUserId { get; set; }
        public ApplicationUser? CreatedByUser { get; set; }
    }
}
