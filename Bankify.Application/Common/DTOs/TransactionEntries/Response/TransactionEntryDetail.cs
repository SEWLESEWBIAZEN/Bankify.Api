using Bankify.Application.Common.DTOs.Accounts.Response;
using Bankify.Application.Common.DTOs.Transactions.Response;
using Bankify.Domain.Models.Shared;

namespace Bankify.Application.Common.DTOs.TransactionEntries.Response
{
    public class TransactionEntryDetail
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public TransactionDetail Transaction { get; set; }
        public int AccountId { get; set; }
        public MinimalAccountDetail Account { get; set; }
        public decimal BalanceBeforeTransaction { get; set; } = 0.00m;
        public decimal BalanceAfterTransaction { get; set; } = 0.00m;
        public decimal Amount { get; set; }
        public EntryType EntryType { get; set; } // Debit or Credit
    }
}
