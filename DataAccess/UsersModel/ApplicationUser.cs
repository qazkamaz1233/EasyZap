using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DataAccess.UserModel
{
    public enum UserRole
    {
        Client = 0,
        Master = 1
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
