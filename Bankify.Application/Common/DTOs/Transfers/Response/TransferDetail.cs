using Bankify.Application.Common.DTOs.Accounts.Response;

namespace Bankify.Application.Common.DTOs.Transfers.Response
{
    public class TransferDetail
    {
        public int Id { get; set; }
        public int TransferedFromId { get; set; }
        public AccountDetail TransferedFrom { get; set; }
        public int TransferredToId { get; set; }
        public AccountDetail TransferedTo { get; set; }
        public decimal AmmountTransfered { get; set; }
        public string TransferStatus { get; set; }
        public DateTime TransferedOn { get; set; }
    }
}
