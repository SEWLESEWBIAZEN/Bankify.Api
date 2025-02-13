using Bankify.Domain.Common.Shared;
using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Shared;

namespace Bankify.Domain.Models.Transactions
{
    public class ATransaction:BaseEntity
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public int TransactionTypeId { get; set; }
        public TransactionType TransactionType { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public TransactionStatus Status {  get; set; }
        public DateTime TransactionDate { get; set; }
       
    }
}

