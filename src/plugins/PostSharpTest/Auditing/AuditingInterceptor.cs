using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using PostSharp.Aspects;

namespace Volo.Abp.Auditing
{
    [Serializable]
    public class AuditingInterceptor : MethodInterceptionAspect
    {
        private readonly AuditingHelper _auditingHelper;
        private readonly AuditingManager _auditingManager;
        
        public AuditingInterceptor()
        {
            _auditingHelper = new AuditingHelper();
            _auditingManager = new AuditingManager();
        }
        
        /// <summary>
        ///   Method invoked <i>instead</i> of the method to which the aspect has been applied.
        /// </summary>
        /// <param name="args">Advice arguments.</param>
        public override async Task OnInvokeAsync(MethodInterceptionArgs args)
        {
            if (!ShouldIntercept(args, out var auditLog, out var auditLogAction))
            {
                await args.ProceedAsync();
                return;
            }

            var stopwatch = Stopwatch.StartNew();

            try
            {
                await args.ProceedAsync();
            }
            catch (Exception ex)
            {
                auditLog.Exceptions.Add(ex);
                throw;
            }
            finally
            {
                stopwatch.Stop();
                auditLogAction.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
                auditLog.Actions.Add(auditLogAction);
            }
        }

        protected virtual bool ShouldIntercept(MethodInterceptionArgs args, out AuditLogInfo auditLog,
            out AuditLogActionInfo auditLogAction)
        {
            auditLog = new AuditLogInfo();
            auditLogAction = new AuditLogActionInfo();

            var auditLogScope = _auditingManager.Current;
            if (auditLogScope == null)
            {
                return false;
            }

            auditLog = auditLogScope.Log;
            auditLogAction = _auditingHelper.CreateAuditLogAction(
                auditLog,
                args.Method.DeclaringType?.GetType(),
                args.Method as MethodInfo, 
                args.Arguments.ToArray()
            );

            return true;
        }
    }
}
