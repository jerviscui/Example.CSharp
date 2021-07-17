namespace Volo.Abp.Auditing
{
    public class AuditLogScope
    {
        public AuditLogInfo Log { get; }

        public AuditLogScope(AuditLogInfo log)
        {
            Log = log;
        }
    }
}