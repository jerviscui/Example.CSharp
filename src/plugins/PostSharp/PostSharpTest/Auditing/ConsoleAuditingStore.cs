using System;
using System.Threading.Tasks;

namespace Volo.Abp.Auditing
{
    [Serializable]
    public class ConsoleAuditingStore
    {
        public ValueTask SaveAsync(AuditLogInfo auditInfo)
        {
            Console.WriteLine(auditInfo.ToString());

            return ValueTask.CompletedTask;
        }
    }
}
