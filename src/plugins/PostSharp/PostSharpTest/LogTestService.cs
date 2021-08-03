using System;
using System.Threading.Tasks;
using PostSharp.Extensibility;
using Volo.Abp.AspNetCore.Auditing;
using Volo.Abp.Auditing;

namespace PostSharpTest
{
    [AbpAuditingMiddleware(AttributeTargetElements = MulticastTargets.Method)]
    [AuditingInterceptor(AttributeTargetElements = MulticastTargets.Method)]
    public class LogTestService
    {
        public Task DoAsync()
        {
            Console.WriteLine("LogTestService.Do");

            return Task.CompletedTask;
        }
    }
}
