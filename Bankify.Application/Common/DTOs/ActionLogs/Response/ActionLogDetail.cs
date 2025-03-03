namespace Bankify.Application.Common.DTOs.ActionLogs.Response
{
    public class ActionLogDetail
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string MainEntity { get; set; }
        public int SubEntityId { get; set; }       
        public string ActionTakenBy { get; set; }
        public DateTime ActionTakenOn { get; set; }
        public string? Description { get; set; }
    }
}
