using System;
using System.Threading.Tasks;
using PostSharp.Aspects;
using Volo.Abp.Auditing;

namespace Volo.Abp.AspNetCore.Auditing
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct |
                    AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event |
                    AttributeTargets.Interface, AllowMultiple = false)]
    [Serializable]
    public class AbpAuditingMiddleware : MethodInterceptionAspect
    {
        private AuditingManager CreateAuditingManager()
        {
            return new();
        }

        /// <summary>
        ///   Method invoked <i>instead</i> of the method to which the aspect has been applied.
        /// </summary>
        /// <param name="args">Advice arguments.</param>
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            //AmbientDataContextAmbientScopeProvider.Begin()
            //Auditing
            //Save
            //Disposing

            var auditingManager = CreateAuditingManager();

            using (var scope = auditingManager.BeginScope())
            {
                try
                {
                    args.Proceed();
                    var exs = auditingManager.Current!.Log.Exceptions;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    scope.SaveAsync();
                }
            }
        }

        /// <summary>
        ///   Method invoked <i>instead</i> of the method to which the aspect has been applied.
        /// </summary>
        /// <param name="args">Advice arguments.</param>
        public override async Task OnInvokeAsync(MethodInterceptionArgs args)
        {
            var auditingManager = CreateAuditingManager();

            using (var scope = auditingManager.BeginScope())
            {
                try
                {
                    await args.ProceedAsync();
                    var exs = auditingManager.Current!.Log.Exceptions;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    await scope.SaveAsync();
                }
            }
        }
    }
}
