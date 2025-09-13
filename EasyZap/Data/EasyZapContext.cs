using EasyZap.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyZap.Data
{
    // для миграций
    public class EasyZapContext : DbContext
    {
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<WorkDay> WorkDays { get; set; }
        public DbSet<MasterLink> MasterLinks { get; set; }
        public EasyZapContext(DbContextOptions<EasyZapContext> options) : base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MasterLink>()
                .HasIndex(x => x.Token)
                .IsUnique();
        }
    }
}
