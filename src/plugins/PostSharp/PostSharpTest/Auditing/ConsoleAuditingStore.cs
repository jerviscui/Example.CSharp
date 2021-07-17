using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

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