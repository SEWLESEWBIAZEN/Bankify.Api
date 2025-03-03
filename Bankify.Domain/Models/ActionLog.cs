
using Bankify.Domain.Models.Shared;

namespace Bankify.Domain.Models
{
    public class ActionLog
    {
        public int Id { get; set; }
        public ActionType Action { get; set; }
        public string MainEntity { get; set; }
        public int SubEntityId { get; set; }      
        public string ActionTakenBy { get; set; }
        public DateTime ActionTakenOn { get; set; }=DateTime.Now;
        public string? Description { get; set; }
    }
}
