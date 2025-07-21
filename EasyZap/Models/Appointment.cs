using System.ComponentModel.DataAnnotations;

namespace EasyZap.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public int MasterId { get; set; }

        public Master Master { get; set; }

        [Required]
        public string ClientName { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public string Service {  get; set; }
    }
}
