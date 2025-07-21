using EasyZap.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyZap.Data
{
    // для миграций
    public class EasyZapContext : DbContext
    {
        public DbSet<Master> Masters { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public EasyZapContext(DbContextOptions<EasyZapContext> options) : base(options)
        {

        }
    }
}
