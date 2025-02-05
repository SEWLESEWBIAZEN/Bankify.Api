using Bankify.Domain.Models.Shared;

namespace Bankify.Domain.Common.Shared
{
    public class BaseEntity
    {
        //Auditlog
        public DateTime RegisteredDate { get; set; }
        public string RegisteredBy { get; set; } = string.Empty;
        public DateTime LastUpdateDate { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public RecordStatus RecordStatus { get; set; }

        public BaseEntity()
        {
            RegisteredDate = DateTime.UtcNow;
            LastUpdateDate = DateTime.UtcNow;
            RecordStatus = RecordStatus.Active;

        }
        public virtual void UpdateAudit(string updateBy)
        {
            LastUpdateDate = DateTime.UtcNow;
            UpdatedBy = SetEmptyString(updateBy);

        }
        public virtual void Register(string updateBy)
        {
            RegisteredDate = DateTime.UtcNow;
            RegisteredBy = SetEmptyString(updateBy);

        }
        protected static string SetEmptyString(string value) => string.IsNullOrEmpty(value) ? string.Empty : value;
    }
}
