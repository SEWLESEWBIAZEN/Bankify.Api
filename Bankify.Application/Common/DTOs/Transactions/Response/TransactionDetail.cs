using Bankify.Domain.Models.Shared;
namespace Bankify.Application.Common.DTOs.Transactions.Response
{
    public class TransactionDetail
    {
        public int Id { get; set; }
        public string Description { get; set; }       
        public TransactionStatus Status { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
