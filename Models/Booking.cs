using System.ComponentModel.DataAnnotations;

namespace JordanTouristGuide.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public string HotelName { get; set; } = string.Empty;

        public int? PropertyId { get; set; }
        public Property? Property { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Required]
        [Range(1, 10)]
        public int NumberOfGuests { get; set; }

        public string? SpecialRequests { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.Now;

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public string Status { get; set; } = "Pending";
    }
}
