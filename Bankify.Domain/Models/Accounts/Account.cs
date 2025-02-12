
using Bankify.Domain.Common.Shared;
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
        
    }
}
