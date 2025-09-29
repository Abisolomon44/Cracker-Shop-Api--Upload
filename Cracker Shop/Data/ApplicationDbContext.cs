using Microsoft.EntityFrameworkCore;
using Cracker_Shop.Models.CommonMasterModels;

namespace Cracker_Shop.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<CompanyMaster> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyMaster>()
                .ToTable("CompanyMaster")
                .HasKey(c => c.CompanyID);

            modelBuilder.Entity<CompanyMaster>()
                .Property(c => c.CompanyCode)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
