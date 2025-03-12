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
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(p => p.User)
                .WithMany(cc => cc.Accounts)
                .HasForeignKey(ph => ph.UserId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.AccountType)
               .WithMany(cc => cc.Accounts)
               .HasForeignKey(ph => ph.AccountTypeId)
               .OnDelete(DeleteBehavior.Restrict);

                entity.Property(p => p.Balance)
                .HasPrecision(38, 10);

            });
            modelBuilder.Entity<Transfer>(entity => {
            entity.HasKey(x => x.Id);

            entity.HasOne(t => t.TransferedFrom)
            .WithMany(a => a.TransfersFrom)
            .HasForeignKey(fh => fh.TransferedFromId)
            .OnDelete(DeleteBehavior.Restrict);

             entity.HasOne(t => t.TransferedTo)
            .WithMany(a => a.TransfersTo)
            .HasForeignKey(fh => fh.TransferredToId)
            .OnDelete(DeleteBehavior.Restrict);

            entity.Property(t => t.AmmountTransfered)
            .HasPrecision(38, 10);

            });            
                                      
            modelBuilder.Entity<AccountType>()
                .Property(tt => tt.InterestRate)
                .HasPrecision(5, 2);

            //transaction entries
            modelBuilder.Entity<TransactionEntry>(entity => { 
                entity.HasKey(t => t.Id);
                entity.HasOne(t => t.Transaction)
                .WithMany(tr => tr.TransactionEntries)
                .HasForeignKey(fh => fh.TransactionId)
                .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(t => t.Account)
                .WithMany(tr => tr.TransactionEntries)
                .HasForeignKey(fh => fh.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.BalanceBeforeTransaction).HasPrecision(25, 5);
                entity.Property(e=>e.Amount).HasPrecision(25, 5);
                entity.Property(e=>e.BalanceAfterTransaction).HasPrecision(25, 5);

            });

            //Authorization          

            modelBuilder.Entity<UserRole>(entity => { 
                entity.HasKey(t => t.Id);
                entity.HasOne(ur => ur.AppRole)
                .WithMany(ar => ar.UserRoles)
                .HasForeignKey(fh => fh.AppRoleId)
                .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(ur => ur.AppUser)
                .WithMany(au => au.UserRoles)
                .HasForeignKey(fh => fh.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<RoleClaim>(entity => { 
                entity.HasKey(e => e.Id);
                entity.HasOne(rc => rc.AppClaim)
                .WithMany(ac => ac.RoleClaims)
                .HasForeignKey(fh => fh.AppClaimId)
                .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(rc => rc.AppRole)
                .WithMany(ar => ar.RoleClaims)
                .HasForeignKey(fh => fh.AppRoleId)
                .OnDelete(DeleteBehavior.Restrict);
            });                            
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
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<TransactionEntry> TransactionEntries { get; set; }
        #endregion

        #region Authorization
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppClaim> AppClaims { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        #endregion
    }

}
