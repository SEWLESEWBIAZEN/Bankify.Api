namespace Bankify.Application.Common.DTOs.Accounts.Request
{
    public class DepositToAccountRequest
    {
        public string AccountNumber { get; set; }=String.Empty;
        public decimal Ammount { get; set; }
    }
}
