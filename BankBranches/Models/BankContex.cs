namespace BankBranches.Models
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using System.Reflection.Metadata;

    public class BankContext : DbContext
    {
        public BankContext()
        {
            
        }
        public DbSet<BankBranch> Branches { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=bank.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Employee>().HasIndex(r => r.CivilId).IsUnique();
            modelBuilder.Entity<Employee>().Property(r => r.CivilId).IsRequired();

        }
    }
}
