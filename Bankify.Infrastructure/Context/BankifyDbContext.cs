using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Bankify.Infrastructure.Context
{
    public class BankifyDbContext(DbContextOptions options) : DbContext(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasOne(p => p.User)
                .WithMany(cc => cc.Accounts)
                .HasForeignKey(ph => ph.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Account>()
               .HasOne(p => p.AccountType)
               .WithMany(cc => cc.Accounts)
               .HasForeignKey(ph => ph.AccountTypeId)
               .OnDelete(DeleteBehavior.Restrict);

        }

        #region Users
        public DbSet<BUser> Users { get; set; }
        #endregion

        #region Accounts
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        #endregion
    }

}
