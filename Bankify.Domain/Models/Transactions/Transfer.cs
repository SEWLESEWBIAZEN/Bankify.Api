using Bankify.Domain.Common.Shared;
using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Shared;

namespace Bankify.Domain.Models.Transactions
{
    public class Transfer:BaseEntity
    {
        public int Id { get; set; }
        public int TransferedFromId { get; set; }
        public Account TransferedFrom { get; set; }
        public int TransferredToId { get; set; }
        public Account TransferedTo { get; set; }
        public decimal AmmountTransfered { get; set; }
        public TransactionStatus TransferStatus { get; set; }
        public DateTime TransferedOn { get; set; }

    }
}
