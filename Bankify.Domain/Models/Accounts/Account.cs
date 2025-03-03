
using Bankify.Domain.Common.Shared;
using Bankify.Domain.Models.Transactions;
using Bankify.Domain.Models.Users;

namespace Bankify.Domain.Models.Accounts
{
    public class Account:BaseEntity
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }=String.Empty;
        public decimal Balance { get; set; } = 0.00m;
        public int AccountTypeId { get; set; }
        public AccountType AccountType { get; set; }        
        public int UserId { get; set; }
        public BUser User { get; set; }
        public string CurrencyCode { get; set; } = "ETB";
        public IEnumerable<ATransaction> Transactions { get; set; }
        public IEnumerable<Transfer> TransfersFrom { get; set; }
        public IEnumerable<Transfer> TransfersTo { get; set; }

    }
}
