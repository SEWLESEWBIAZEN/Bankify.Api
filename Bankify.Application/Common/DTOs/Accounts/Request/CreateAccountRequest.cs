namespace Bankify.Application.Common.DTOs.Accounts.Request
{
    public class CreateAccountRequest
    {
       // public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public int AccountTypeId { get; set; }
        public int? UserId { get; set; }
        public string? CurrencyCode { get; set; }
    }
}
