using Bankify.Domain.Common.Shared;
using Bankify.Domain.Models.Accounts;

namespace Bankify.Domain.Models.Branches
{
    public class Branch : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public IEnumerable<Account> Accounts { get; set; }

    }
}
