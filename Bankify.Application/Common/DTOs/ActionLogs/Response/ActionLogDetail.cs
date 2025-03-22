namespace Bankify.Application.Common.DTOs.ActionLogs.Response
{
    public class ActionLogDetail
    {
        public int Id { get; set; }
        public string Action { get; set; }=String.Empty;
        public string MainEntity { get; set; }=String.Empty;
        public int SubEntityId { get; set; }=0;       
        public string ActionTakenBy { get; set; }=String.Empty;
        public DateTime ActionTakenOn { get; set; }
        public string? Description { get; set; }
    }
}
