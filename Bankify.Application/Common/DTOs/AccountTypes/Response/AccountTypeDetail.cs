namespace Bankify.Application.Common.DTOs.AccountTypes.Response
{
    public class AccountTypeDetail
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public string Name { get; set; } 
        public string Description { get; set; } 
        public decimal InterestRate { get; set; }

    }
}
