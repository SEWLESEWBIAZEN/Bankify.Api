using Bankify.Application.Common.DTOs.Accounts.Response;
using Bankify.Application.Common.DTOs.TransactionTypes.Response;
using Bankify.Domain.Models.Shared;

namespace Bankify.Application.Common.DTOs.Transactions.Response
{
    public class TransactionDetail
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public int TransactionTypeId { get; set; }
        public TransactionTypeDetail TransactionType { get; set; }
        public decimal BalanceBeforeTransaction { get; set; }
        public decimal BalanceAfterTransaction { get; set; }
        public int AccountId { get; set; }
        public AccountDetail Account { get; set; }
        public TransactionStatus Status { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
