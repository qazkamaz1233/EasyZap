using EasyZap.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyZap.Data
{
    public class EasyZapContext : DbContext
    {
        public DbSet<Master> Masters { get; set; }

        public EasyZapContext(DbContextOptions<EasyZapContext> options) : base(options)
        {

        }
    }
}
