using Bankify.Domain.Common.Shared;

namespace Bankify.Domain.Models.Accounts
{
    public class AccountType : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public IEnumerable<Account> Accounts { get; set; }
       
    }
}
