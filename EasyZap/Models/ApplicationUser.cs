using System.ComponentModel.DataAnnotations;

namespace EasyZap.Models
{
    public enum UserRole
    {
        Client,
        Master
    }

    public class ApplicationUser
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
