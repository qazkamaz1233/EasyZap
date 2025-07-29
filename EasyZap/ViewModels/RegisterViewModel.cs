using System.ComponentModel.DataAnnotations;
using EasyZap.Models;

namespace EasyZap.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        [Required]
        public UserRole Role { get; set; }
    }
}
