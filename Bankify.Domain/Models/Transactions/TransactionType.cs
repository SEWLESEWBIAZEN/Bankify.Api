using Bankify.Domain.Common.Shared;

namespace Bankify.Domain.Models.Transactions
{
    public class TransactionType:BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public IEnumerable<ATransaction> Transactions { get; set; }

    }
}
