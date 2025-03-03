namespace Bankify.Application.Common.DTOs.Accounts.Request
{
    public class DepositToAccountRequest
    {
        public string AccountNumber { get; set; }
        public decimal Ammount { get; set; }
    }
}
