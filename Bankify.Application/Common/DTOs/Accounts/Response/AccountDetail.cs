using Bankify.Application.Common.DTOs.AccountTypes.Response;
using Bankify.Application.Common.DTOs.Users.Response;

namespace Bankify.Application.Common.DTOs.Accounts.Response
{
    public class AccountDetail
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; } 
        public int AccountTypeId { get; set; }
        public AccountTypeDetail AccountType { get; set; }
        public int UserId { get; set; }
        public UserDetail User { get; set; }
    }
}
