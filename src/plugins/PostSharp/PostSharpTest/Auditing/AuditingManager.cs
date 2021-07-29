using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Threading;

namespace Volo.Abp.Auditing
{
    [Serializable]
    public class AuditingManager
    {
        private const string AmbientContextKey = "Volo.Abp.Auditing.AuditLogScope";

        private readonly AmbientDataContextAmbientScopeProvider<AuditLogScope> _ambientScopeProvider;
        private readonly AuditingHelper _auditingHelper;
        private readonly ConsoleAuditingStore _auditingStore;

        public AuditingManager()
        {
            _ambientScopeProvider = AmbientDataContextAmbientScopeProvider<AuditLogScope>.Instance;
            _auditingHelper = new AuditingHelper();
            _auditingStore = new ConsoleAuditingStore();
        }

        public AuditLogScope? Current => _ambientScopeProvider.GetValue(AmbientContextKey);

        public IAuditLogSaveHandle BeginScope()
        {
            var ambientScope = _ambientScopeProvider.BeginScope(
                AmbientContextKey,
                new AuditLogScope(_auditingHelper.CreateAuditLogInfo())
            );

            Debug.Assert(Current != null, "Current != null");

            return new DisposableSaveHandle(this, ambientScope, Current.Log, Stopwatch.StartNew());
        }

        protected virtual void BeforeSave(DisposableSaveHandle saveHandle)
        {
            saveHandle.StopWatch.Stop();
            saveHandle.AuditLog.ExecutionDuration = Convert.ToInt32(saveHandle.StopWatch.Elapsed.TotalMilliseconds);
            //ExecutePostContributors(saveHandle.AuditLog);
            //MergeEntityChanges(saveHandle.AuditLog);
        }

        protected virtual async Task SaveAsync(DisposableSaveHandle saveHandle)
        {
            BeforeSave(saveHandle);

            await _auditingStore.SaveAsync(saveHandle.AuditLog);
        }

        protected class DisposableSaveHandle : IAuditLogSaveHandle
        {
            public AuditLogInfo AuditLog { get; }
            public Stopwatch StopWatch { get; }

            private readonly AuditingManager _auditingManager;
            private readonly IDisposable _scope;

            public DisposableSaveHandle(
                AuditingManager auditingManager,
                IDisposable scope,
                AuditLogInfo auditLog,
                Stopwatch stopWatch)
            {
                _auditingManager = auditingManager;
                _scope = scope;
                AuditLog = auditLog;
                StopWatch = stopWatch;
            }

            public async Task SaveAsync()
            {
                await _auditingManager.SaveAsync(this);
            }

            public void Dispose()
            {
                _scope.Dispose();
            }
        }
    }
}