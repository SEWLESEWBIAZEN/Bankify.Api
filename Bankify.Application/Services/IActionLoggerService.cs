using Bankify.Domain.Models;
using Bankify.Domain.Models.Shared;
using Bankify.Infrastructure.Context;

namespace Bankify.Application.Services
{
    public interface IActionLoggerService
    {
        Task TakeActionLog(ActionType action, string mainEntity, int subEntityId,  string actionTakenBy, string? description);
    }

    public class ActionLoggerService : IActionLoggerService
    {
        private readonly BankifyDbContext _bankifyDbContext;
        public ActionLoggerService(BankifyDbContext bankifyDbContext)
        {
            _bankifyDbContext = bankifyDbContext;
        }
        
        public async Task TakeActionLog(ActionType action, string mainEntity, int subEntityId,string actionTakenBy, string? description)
        {
            var newActionlog = new ActionLog
            {
                Action = action,
                MainEntity = mainEntity,
                SubEntityId = subEntityId,              
                ActionTakenBy = actionTakenBy,
                ActionTakenOn = DateTime.Now,
                Description = description
            };
            _bankifyDbContext.Add(newActionlog);
            await _bankifyDbContext.SaveChangesAsync();
        }
    }
}
