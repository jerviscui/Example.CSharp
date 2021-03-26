using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace Volo.Abp.Auditing
{
    //transient
    [Serializable]
    public class AuditingHelper
    {
        public virtual AuditLogInfo CreateAuditLogInfo()
        {
            var auditInfo = new AuditLogInfo
            {
                ApplicationName = "PostSharpTest",
                TenantId = null,
                TenantName = "",
                UserId = null,
                UserName = "",
                ClientId = "",
                CorrelationId = "",
                //ImpersonatorUserId = AbpSession.ImpersonatorUserId, //TODO: Impersonation system is not available yet!
                //ImpersonatorTenantId = AbpSession.ImpersonatorTenantId,
                ExecutionTime = DateTime.Now
            };

            //ExecutePreContributors(auditInfo);

            return auditInfo;
        }

        public virtual AuditLogActionInfo CreateAuditLogAction(
            AuditLogInfo auditLog,
            Type type,
            MethodInfo method,
            object[] arguments)
        {
            return CreateAuditLogAction(auditLog, type, method, CreateArgumentsDictionary(method, arguments));
        }

        public virtual AuditLogActionInfo CreateAuditLogAction(
            AuditLogInfo auditLog,
            Type type,
            MethodInfo method,
            IDictionary<string, object> arguments)
        {
            var actionInfo = new AuditLogActionInfo
            {
                ServiceName = type != null ? type.FullName : "",
                MethodName = method.Name,
                Parameters = SerializeConvertArguments(arguments),
                ExecutionTime = DateTime.Now
            };

            //TODO Execute contributors

            return actionInfo;
        }

        protected virtual string SerializeConvertArguments(IDictionary<string, object> arguments)
        {
            try
            {
                if (arguments.IsNullOrEmpty())
                {
                    return "{}";
                }

                var dictionary = new Dictionary<string, object>();

                foreach (var argument in arguments)
                {
                    dictionary[argument.Key] = argument.Value;
                }

                return JsonSerializer.Serialize(dictionary);
            }
            catch (Exception ex)
            {
                return "{}";
            }
        }

        protected virtual Dictionary<string, object> CreateArgumentsDictionary(MethodInfo method, object[] arguments)
        {
            var parameters = method.GetParameters();
            var dictionary = new Dictionary<string, object>();

            for (var i = 0; i < parameters.Length; i++)
            {
                dictionary[parameters[i].Name] = arguments[i];
            }

            return dictionary;
        }
    }
}
