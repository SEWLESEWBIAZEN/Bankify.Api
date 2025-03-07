﻿using Bankify.Domain.Models;
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
            modelBuilder.Entity<Account>()
                .Property(p => p.Balance)
                .HasPrecision(38, 10);

            modelBuilder.Entity<AccountType>()
                .Property(tt => tt.InterestRate)
                .HasPrecision(5, 2);

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
            modelBuilder.Entity<Transfer>()
                .Property(t=>t.AmmountTransfered)
                .HasPrecision(38, 10);

            //transaction entries
            modelBuilder.Entity<TransactionEntry>()
                .HasOne(t => t.Transaction)
                .WithMany(tr=>tr.TransactionEntries)
                .HasForeignKey(fh => fh.TransactionId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TransactionEntry>()
                .HasOne(t => t.Account)
                .WithMany(tr=>tr.TransactionEntries)
                .HasForeignKey(fh => fh.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
            
            //Authorization           

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.AppRole)
                .WithMany(ar => ar.UserRoles)
                .HasForeignKey(fh => fh.AppRoleId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserRole>()
                .HasOne(ur=>ur.AppUser)
                .WithMany(au=>au.UserRoles)
                .HasForeignKey(fh=>fh.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<RoleClaim>()
                .HasOne(rc => rc.AppClaim)
                .WithMany(ac => ac.RoleClaims)
                .HasForeignKey(fh => fh.AppClaimId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RoleClaim>()
                .HasOne(rc => rc.AppRole)
                .WithMany(ar => ar.RoleClaims)
                .HasForeignKey(fh => fh.AppRoleId)
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
