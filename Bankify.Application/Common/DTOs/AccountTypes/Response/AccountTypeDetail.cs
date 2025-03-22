namespace Bankify.Application.Common.DTOs.AccountTypes.Response
{
    public class AccountTypeDetail
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }=String.Empty;
        public string Name { get; set; } =String.Empty;
        public string Description { get; set; } =String.Empty;
        public decimal InterestRate { get; set; }

    }
}
