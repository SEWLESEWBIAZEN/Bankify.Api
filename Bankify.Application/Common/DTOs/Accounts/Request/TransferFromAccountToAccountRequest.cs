
namespace Bankify.Application.Common.DTOs.Accounts.Request
{
    public class TransferFromAccountToAccountRequest
    {       
        public string TransferredFromAccountNumber { get; set; }=String.Empty;     
        public string TransferredToAccountNumber { get; set; } =String.Empty;    
        public decimal AmmountTransfered { get; set; }       

    }
}
