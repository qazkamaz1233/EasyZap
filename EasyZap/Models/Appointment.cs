using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyZap.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [NotMapped]
        public Master Master { get; set; }

        [Required]
        public int MasterId { get; set; }


        [Required(ErrorMessage = "Имя клиента обязателено")]
        public string ClientName { get; set; }

        [Required(ErrorMessage = "Время записи обязательно")]
        public DateTime DateTime { get; set; }

        [Required(ErrorMessage = "Укажите предоставляемую услугу")]
        public string Service {  get; set; }
    }
}
