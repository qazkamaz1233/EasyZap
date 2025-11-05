using DataAccess.UserModel;
using DataAccess.EntityModel;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    // для миграций
    public class EasyZapContext : DbContext
    {
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<WorkDay> WorkDays { get; set; }
        public EasyZapContext(DbContextOptions<EasyZapContext> options) : base(options)
        {

        }
    }
}
