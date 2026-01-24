namespace Bankify.Application.Common.DTOs.TransactionTypes.Request
{
    public class UpdateTransactionTypeRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
