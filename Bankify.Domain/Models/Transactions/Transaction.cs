using Bankify.Domain.Common.Shared;
using Bankify.Domain.Models.Shared;

namespace Bankify.Domain.Models.Transactions
{ 
    public class ATransaction : BaseEntity
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Reference { get; set; }=string.Empty;
        public TransactionStatus Status { get; set; }
        public ICollection<TransactionEntry> TransactionEntries { get; set; }= new List<TransactionEntry>();

    }
}

