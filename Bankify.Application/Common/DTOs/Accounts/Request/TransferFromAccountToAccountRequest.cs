
namespace Bankify.Application.Common.DTOs.Accounts.Request
{
    public class TransferFromAccountToAccountRequest
    {       
        public string TransferredFromAccountNumber { get; set; }       
        public string TransferredToAccountNumber { get; set; }       
        public decimal AmmountTransfered { get; set; }       

    }
}
