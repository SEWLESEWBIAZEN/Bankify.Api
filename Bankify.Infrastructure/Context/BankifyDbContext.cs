using Bankify.Domain.Models;
using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Transactions;
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

            modelBuilder.Entity<ATransaction>()
                .HasOne(t=>t.Account)
                .WithMany(a=>a.Transactions)
                .HasForeignKey(fh=>fh.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ATransaction>()
                .HasOne(t=>t.TransactionType)
                .WithMany(a=>a.Transactions)
                .HasForeignKey(fh=>fh.TransactionTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Transfer>()
                .HasOne(t => t.TransferedFrom)
                .WithMany(a => a.TransfersFrom)
                .HasForeignKey(fh => fh.TransferedFromId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Transfer>()
                .HasOne(t=>t.TransferedTo)
                .WithMany(a=>a.TransfersTo)
                .HasForeignKey(fh=>fh.TransferredToId) 
                .OnDelete(DeleteBehavior.Restrict);



        }

        #region Users
        public DbSet<BUser> Users { get; set; }
        #endregion

        #region Accounts
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        #endregion

        #region ActionLog
        public DbSet<ActionLog> ActionLogs { get; set; }
        #endregion

        #region Transactions
        public DbSet<ATransaction> TransactionLogs { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        #endregion
    }

}
