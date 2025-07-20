using System.ComponentModel.DataAnnotations;

namespace EasyZap.Models
{
    public class Master
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Неверный формат email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        public string Name { get; set; }

        public string Service { get; set; }

        public string Schedule { get; set; }
    }
}
