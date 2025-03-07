using Bankify.Domain.Common.Shared;
using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Shared;

namespace Bankify.Domain.Models.Transactions
{
    public class TransactionEntry:BaseEntity
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public ATransaction Transaction { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public decimal BalanceBeforeTransaction { get; set; } = 0.00m;
        public decimal BalanceAfterTransaction { get; set; } = 0.00m;
        public decimal Amount { get; set; }
        public EntryType EntryType { get; set; } // Debit or Credit
    }
}
