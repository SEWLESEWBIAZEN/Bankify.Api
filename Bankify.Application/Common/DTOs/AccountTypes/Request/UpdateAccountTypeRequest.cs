namespace Bankify.Application.Common.DTOs.AccountTypes.Request
{
    public class UpdateAccountTypeRequest
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal InterestRate { get; set; }
    }
}