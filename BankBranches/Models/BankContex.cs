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

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=bank.db");
        }
    }
}
