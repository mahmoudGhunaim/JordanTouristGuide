using System.ComponentModel.DataAnnotations;

namespace JordanTouristGuide.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        public DateTime SubmittedDate { get; set; } = DateTime.Now;
    }
}
