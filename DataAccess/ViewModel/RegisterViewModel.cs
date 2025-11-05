using System.ComponentModel.DataAnnotations;
using DataAccess.UserModel;

namespace DataAccess.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите Email")]
        [EmailAddress(ErrorMessage = "Введите корректный Email")]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        [Required]
        public UserRole Role { get; set; }
    }
}
